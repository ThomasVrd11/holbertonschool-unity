using UnityEngine;
using UnityEngine.InputSystem;

public class WebXRMovementManager : MonoBehaviour
{
    [Header("References")]
    public Transform movementReference;  // use a stable forward object (PlayerRoot or dummy)
    public Transform cameraPivot;        // object that rotates left/right slightly with mouse
    public Transform cameraHolder;       // object that moves in/out for zoom

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Edge Tilt Settings")]
    public float edgeThreshold = 100f;
    public float maxYawAngle = 10f;
    public float rotationSmoothSpeed = 5f;

    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 10f;

    private CharacterController controller;
    private float currentZoom;
    private float targetYaw = 0f;
    private float currentYaw = 0f;

    [Header("Ball steering")]
    private bool isSteeringBall = false;
    private Rigidbody currentBall;
    public float ballSteerForce = 10f;


    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentZoom = 0f;
    }

    void Update()
    {
        // ------------------- CAMERA TILT -------------------
        Vector2 mousePos = Mouse.current.position.ReadValue();
        float screenWidth = Screen.width;

        if (mousePos.x <= edgeThreshold)
        {
            float t = 1f - (mousePos.x / edgeThreshold);
            targetYaw = Mathf.Lerp(0f, -maxYawAngle, t);
        }
        else if (mousePos.x >= screenWidth - edgeThreshold)
        {
            float t = (mousePos.x - (screenWidth - edgeThreshold)) / edgeThreshold;
            targetYaw = Mathf.Lerp(0f, maxYawAngle, t);
        }
        else
        {
            targetYaw = 0f;
        }

        currentYaw = Mathf.Lerp(currentYaw, targetYaw, Time.deltaTime * rotationSmoothSpeed);
        cameraPivot.localRotation = Quaternion.Euler(0f, currentYaw, 0f);

        // ------------------- ZOOM -------------------
        currentZoom +=
            (Keyboard.current.downArrowKey.isPressed ? 1f :
             Keyboard.current.upArrowKey.isPressed ? -1f : 0f) * zoomSpeed * Time.deltaTime;

        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        Vector3 localPos = cameraHolder.localPosition;
        localPos.z = -currentZoom;
        cameraHolder.localPosition = localPos;

        // ------------------- MOVEMENT -------------------
        if (!isSteeringBall)
        {
            Vector3 input = new Vector3(
                Keyboard.current.aKey.isPressed ? -1 : Keyboard.current.dKey.isPressed ? 1 : 0,
                0,
                Keyboard.current.sKey.isPressed ? -1 : Keyboard.current.wKey.isPressed ? 1 : 0
            );

            Vector3 direction = movementReference.forward * input.z + movementReference.right * input.x;
            direction.y = 0f;

            controller.Move(direction.normalized * moveSpeed * Time.deltaTime);
        }
        else if (currentBall != null)
        {
            float steer = 0f;
            if (Keyboard.current.leftArrowKey.isPressed) steer = -1f;
            if (Keyboard.current.rightArrowKey.isPressed) steer = 1f;

            Vector3 lateral = movementReference.right * steer * ballSteerForce;
            currentBall.AddForce(lateral, ForceMode.Acceleration);
        }
    }

    public void EnterBallSteering(Rigidbody ballRb)
    {
        isSteeringBall = true;
        currentBall = ballRb;
    }
    public void ExitBallSteering()
    {
        isSteeringBall = false;
        currentBall = null;
    }

}
