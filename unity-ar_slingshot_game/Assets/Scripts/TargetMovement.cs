using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TargetMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _directionChangeInterval = 2f;
    private Vector3 _direction;
    private Transform _planeTransform;
    private ARPlane _plane;
    private List<Vector2> _boundary = new List<Vector2>();
    private float _timer = 0f;

    void Update()
    {
        if (_planeTransform == null)
            return;

        _timer += Time.deltaTime;
        if (_timer >= _directionChangeInterval)
        {
            SetRandomDirection();
            _timer = 0f;
        }

        Vector3 localMove = _direction * _speed * Time.deltaTime;
        Vector3 worldMove = _planeTransform.TransformDirection(localMove);
        Vector3 move = new Vector3(worldMove.x, 0f, worldMove.z);
        Vector3 nextPosition = transform.position + move;
        nextPosition.y = _planeTransform.position.y + 0.01f;

        Vector2 nextPosition2D = new Vector2(_planeTransform.InverseTransformPoint(nextPosition).x,
                                              _planeTransform.InverseTransformPoint(nextPosition).z);
        if (IsInsideBoundary(nextPosition2D))
        {
            transform.position = nextPosition;
            transform.rotation = Quaternion.LookRotation(move);
        }
        else
        {
            SetRandomDirection();
        }
    }
    /// <summary>
    /// Initialises the target movement with the plane transform and ARPlane.
    /// </summary>
    /// <param name="plane"></param>
    /// <param name="arPlane"></param>
    public void Initialise(Transform plane, ARPlane arPlane)
    {
        _planeTransform = plane;
        _plane = arPlane;
        _boundary.Clear();

        foreach (var point in _plane.boundary)
        {
            _boundary.Add(point);
        }
        SetRandomDirection();
    }

    /// <summary>
    /// Sets a random direction for the target to move in.
    /// </summary>
    private void SetRandomDirection()
    {
        _direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

    /// <summary>
    ///  Check if inside boundary using Ray Casting algorithm, thanks internet again, too much math for me.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private bool IsInsideBoundary(Vector2 point)
    {
        int count = _boundary.Count;
        bool inside = false;

        for (int i = 0, j = count - 1; i < count; j = i++)
        {
            Vector2 vi = _boundary[i];
            Vector2 vj = _boundary[j];

            bool intersect = ((vi.y > point.y) != (vj.y > point.y)) &&
                             (point.x < (vj.x - vi.x) * (point.y - vi.y) / (vj.y - vi.y + 0.0001f) + vi.x);

            if (intersect)
                inside = !inside;
        }

        return inside;
    }

}
