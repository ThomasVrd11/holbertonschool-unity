using UnityEngine;

public class Pin : MonoBehaviour
{
    public float fallAngleThreshold = 45f;
    public bool hasFallen { get; private set; } = false;

    public delegate void OnPinFallen(Pin pin);
    public static event OnPinFallen PinFallen;

    void Update()
    {
        if (hasFallen) return;

        float tilt = Vector3.Angle(Vector3.up, transform.up);
        if (tilt > fallAngleThreshold)
        {
            hasFallen = true;
            PinFallen?.Invoke(this);
        }
    }
}
