using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.ARFoundation;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SlingshotAmmo : MonoBehaviour
{
    [SerializeField] private Camera _arCamera;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ARRaycastManager _arRaycastManager;

    [SerializeField] private float _resetY = -1f;
    [SerializeField] private float _force = 10f;
    [SerializeField] private TrailRenderer _trailRenderer;

    // Line renderer trajectory parameters
    [SerializeField] private LineRenderer _lineRenderer;
    private int _trajectoryPointCount = 30;
    private float _trajectoryTimeStep = 0.05f;


    // Dragging parameters
    private Vector2 _startDragPosition;
    private bool _isDragging = false;
    //private static List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private Touchscreen _touchscreen;


    private void Start()
    {
        _touchscreen = Touchscreen.current;
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
        }
        if (_arCamera == null)
        {
            //Debugger.ShowText("No AR Camera found, using main camera instead.");
            _arCamera = Camera.main;
        }
        if (_arRaycastManager == null)
        {
            _arRaycastManager = FindFirstObjectByType<ARRaycastManager>();
            if (_arRaycastManager == null)
            {
                //Debugger.ShowText("No AR Raycast Manager found, disabling slingshot ammo.");
                enabled = false;
                return;
            }
        }

        if (_lineRenderer == null)
        {
            //Debugger.ShowText("No Line Renderer found, searching for it.");
            _lineRenderer = GetComponent<LineRenderer>();
            if (_lineRenderer == null)
            {
                //Debugger.ShowText("No Line Renderer found, disabling.");
                enabled = false;
                return;
            }
        }

        ResetAmmo();
    }

    private void Update()
    {
        HandleToucheInput();
        CheckOutOfBonds();
        //CheckPlaneHit();
    }
    /// <summary>
    /// Handles the collision events for the ammo.
    /// </summary>
    /// <param name="collision">Collision data</param>
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Target"))
        {
            ResetAmmo();
            Destroy(collision.gameObject);
            EventManager.Instance.TriggerScored();
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            EventManager.Instance.AudioManager.PlayTargetMiss();
            ResetAmmo();
        }
    }
    #region Touch Input Handling
    /// <summary>
    /// Handles touch input for dragging and launching the ammo.
    /// </summary>
    private void HandleToucheInput()
    {
        if (_touchscreen == null)
            return;

        TouchControl touch = _touchscreen.primaryTouch;
        RaycastHit hit;

        if (!_isDragging)
        {
            if (touch.press.wasPressedThisFrame)
            {
                Vector2 touchPos = touch.position.ReadValue();
                Ray ray = _arCamera.ScreenPointToRay(touchPos);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.GetComponentInParent<SlingshotAmmo>() == this)
                    {
                        _startDragPosition = touchPos;
                        _isDragging = true;
                        _rb.isKinematic = true;
                    }
                }
            }
        }
        else
        {
            Vector2 currentTouchPos = touch.position.ReadValue();

            if (touch.press.isPressed)
            {
                Vector3 dragWorldPos = _arCamera.ScreenToWorldPoint(new Vector3(currentTouchPos.x, currentTouchPos.y, 0.5f));
                transform.position = dragWorldPos;

                //calculate velocity based on drag
                HandleTrajectoryPrevis(currentTouchPos);
            }

            if (_touchscreen.primaryTouch.press.wasReleasedThisFrame)
            {

                _rb.isKinematic = false;
                _rb.linearVelocity = TargetVelocityAndDirection(currentTouchPos);
                _isDragging = false;
                _lineRenderer.enabled = false;
                EventManager.Instance.TriggerAmmoLaunched();
            }
        }
    }
    #endregion
    #region Tools and Utilities
    /// <summary>
    /// Handles the trajectory preview based on the current touch position.
    /// </summary>
    /// <param name="currentTouchPos">current touch position</param>
    private void HandleTrajectoryPrevis(Vector2 currentTouchPos)
    {
        Vector3 launchVelocity = TargetVelocityAndDirection(currentTouchPos);

        ShowTrajectory(transform.position, launchVelocity);
    }

    /// <summary>
    /// Calculates the target velocity and direction based on the drag distance.
    /// </summary>
    /// <param name="currentTouchPos">current touch position</param>
    /// <returns>vector 3 launch velocity and direction</returns>
    private Vector3 TargetVelocityAndDirection(Vector2 currentTouchPos)
    {
        Vector2 dragVector = _startDragPosition - currentTouchPos;
        Vector3 forceDir = new Vector3(dragVector.x, dragVector.y, 0f).normalized;
        Vector3 worldForceDir = _arCamera.transform.TransformDirection(forceDir);
        Vector3 launchVelocity = worldForceDir * dragVector.magnitude * _force * 0.001f;
        return launchVelocity;
    }

    /// <summary>
    /// Shows the trajectory of the ammo based on the start position and velocity.
    /// </summary>
    /// <param name="startPos">starting position of the drag</param>
    /// <param name="velocity">launch velocity</param>
    private void ShowTrajectory(Vector3 startPos, Vector3 velocity)
    {
        // set the first point of the trajectory
        _lineRenderer.enabled = true;
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, startPos);

        Vector3 previousPoint = startPos;

        for (int i = 0; i < _trajectoryPointCount; i++)
        {
            float t = i * _trajectoryTimeStep;
            /*  
                The trajectory is calculated using the formula: 
                t = simulated time 
                velocity * t = how far the ammo would move without gravity
                + 0.5 * gravity * t² = basic physics formula for gravity’s downward curve
            */
            Vector3 point = startPos + velocity * t + 0.5f * Physics.gravity * (t * t);

            // determine the movement vector between the previous point and the current point
            // and check if it collides with any objects
            Vector3 direction = point - previousPoint;
            float distance = direction.magnitude;
            if (Physics.Raycast(previousPoint, direction.normalized, out RaycastHit hit, distance))
            {
                // if it collides, stop drawing
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(i + 1, hit.point);
                break;
            }
            //no collision, continue drawing the trajectory
            _lineRenderer.positionCount++;
            _lineRenderer.SetPosition(i + 1, point);
            previousPoint = point;
        }
    }

    /// <summary>
    /// Checks if the ammo is out of bounds and resets it if necessary.
    /// </summary>
    private void CheckOutOfBonds()
    {
        if (transform.position.y < _resetY)
        {
            EventManager.Instance.AudioManager.PlayTargetMiss();
            ResetAmmo();
        }
    }

    // /// <summary>
    // /// Checks if the ammo hits a plane and resets it if it does.
    // /// </summary>
    // private void CheckPlaneHit()
    // {
    //     if (!_isDragging && _rb.linearVelocity.y <= 0f)
    //     {
    //         Vector3 screenPos = _arCamera.WorldToScreenPoint(transform.position);
    //         if (_arRaycastManager.Raycast(screenPos, _hits, TrackableType.PlaneWithinPolygon))
    //         {
    //             float hitDistance = Vector3.Distance(transform.position, _hits[0].pose.position);
    //             if (hitDistance < 0.05f)
    //             {
    //                 ResetAmmo();
    //             }
    //         }
    //     }
    // }

    #endregion
    #region Actions
    /// <summary>
    /// Resets the ammo position and state.
    /// </summary>
    private void ResetAmmo()
    {
        if (_trailRenderer != null)
        {
            _trailRenderer.Clear();
            _trailRenderer.enabled = false;
        }
        if (!_arCamera)
            return;
        EventManager.Instance.CheckEndGame();
        if (EventManager.Instance.GameManager.GetAmmoCount() > 0)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, .5f);
            Vector3 worldPos = _arCamera.ScreenToWorldPoint(screenCenter);


            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            _rb.isKinematic = true;

            transform.position = worldPos;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Spawns the ammo and resets its state.
    /// </summary>
    public void Spawn()
    {
        ResetAmmo();
    }
    #endregion
}
