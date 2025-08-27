using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public float boostMultiplier = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.attachedRigidbody) return;
        Rigidbody rb = other.attachedRigidbody;

        Vector3 currentVelocity = rb.linearVelocity;

        rb.linearVelocity = currentVelocity * boostMultiplier;

    }

}
