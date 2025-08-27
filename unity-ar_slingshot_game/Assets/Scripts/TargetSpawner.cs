using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private int _targetNumber = 5;
    [SerializeField] private float _spawnYOffset = 0.01f;

    private bool _hasSpawned = false;
    private List<GameObject> _targetList = new List<GameObject>();

    private void Start()
    {
        EventManager.Instance.OnResetGame += ResetTargets;
        _targetList.Clear();
	}
    private void OnDestroy()
    {
        EventManager.Instance.OnResetGame -= ResetTargets;
    }
    
	void Update()
    {
        if (!_hasSpawned && ARPlaneSelector.SelectedPlane != null)
        {
            _hasSpawned = true;
            SpawnTargets();
        }
    }

    /// <summary>
    /// Resets the targets by destroying all currently spawned targets and clearing the target list.
    /// </summary>
    private void ResetTargets()
    {
        foreach (GameObject target in _targetList)
        {
            if (target == null) continue;
            Destroy(target);
        }
        _targetList.Clear();
        _hasSpawned = false;
    }

    /// <summary>
    /// Spawns a specified number of targets on the selected AR plane.
    /// </summary>
    private void SpawnTargets()
    {
        ARPlane plane = ARPlaneSelector.SelectedPlane;

        Vector3 planeCenter = plane.transform.position;

        for (int i = 0; i < _targetNumber; i++)
        {
            Vector3 offset = GetRandomPositionOnPlane(plane);
            Vector3 spawnPosition = planeCenter + plane.transform.TransformVector(offset);
            spawnPosition.y += _spawnYOffset;

            GameObject target = Instantiate(_targetPrefab, spawnPosition, Quaternion.identity);


            target.GetComponent<TargetMovement>().Initialise(plane.transform, plane);
            _targetList.Add(target);
        }
    }

    /// <summary>
    /// Generates a random position on the given AR plane.
    /// </summary>
    /// <param name="plane"></param>
    /// <returns></returns>
    private Vector3 GetRandomPositionOnPlane(ARPlane plane)
    {
        var boundary = plane.boundary;
        if (boundary.Length < 3)
        {
            return Vector3.zero;
        }

        int i1 = Random.Range(0, boundary.Length);
        int i2 = (i1 + 1) % boundary.Length;

        Vector2 a = boundary[i1];
        Vector2 b = boundary[i2];
        Vector2 c = Vector2.zero;

        float r1 = Random.value;
        float r2 = Random.value;

        // Barycentric interpolation to get a point inside the triangle. thanks google
        if (r1 + r2 > 1f)
        {
            r1 = 1f - r1;
            r2 = 1f - r2;
        }

        Vector2 point = a + r1 * (b - a) + r2 * (c - a);
        return new Vector3(point.x, 0f, point.y);
    }
}
