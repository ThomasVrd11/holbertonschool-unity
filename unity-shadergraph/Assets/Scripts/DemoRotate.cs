using UnityEngine;

public class DemoRotate : MonoBehaviour
{
    private Transform _t;
    void Start()
    {
        _t = this.transform;
    }

    
    void Update()
    {
        _t.Rotate(Vector3.up, 20 * Time.deltaTime, Space.World);
    }
}
