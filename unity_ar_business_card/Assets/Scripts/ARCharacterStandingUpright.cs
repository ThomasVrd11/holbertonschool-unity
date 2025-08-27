using UnityEngine;

public class ARCharacterStandingUpright : MonoBehaviour
{
    [SerializeField] private Transform imageTarget; // assign in inspector or find by tag/name
    public float heightOffset = 20f;
    void Start()
    {
        Input.gyro.enabled = true;
    }

    void LateUpdate()
    {
        if (imageTarget == null) return;

        // 1. Use the image target's position
        Vector3 targetPos = imageTarget.position;
        targetPos.y -= heightOffset;
        transform.position = targetPos;

        // 2. Get the device's attitude (orientation)
        Quaternion deviceAttitude = Input.gyro.attitude;

        // 3. Convert to Unity coordinate system (gyro is right-handed, Unity is left-handed)
        Quaternion unityRotation = new Quaternion(
            -deviceAttitude.x,
            -deviceAttitude.y,
             deviceAttitude.z,
             deviceAttitude.w
        );

        // 4. Convert that to a usable "up" vector
        Vector3 deviceUp = unityRotation * Vector3.up;

        // 5. Project the image target's forward vector onto the horizontal plane defined by device's up
        Vector3 fixedForward = Vector3.back;
        Vector3 forwardProjected = Vector3.ProjectOnPlane(fixedForward, deviceUp).normalized;

        // 6. Apply upright rotation
        if (forwardProjected.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(forwardProjected, deviceUp);
        }
    }
}
