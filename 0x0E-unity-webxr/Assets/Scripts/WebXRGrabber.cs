using UnityEngine;

public class GenericGrabber : MonoBehaviour
{
    [Tooltip("Name of input button, ex: 'Fire1' or 'TriggerLeft' if mapped")]
    public string grabInputName = "Fire1";

    private GrabbableBall heldBall;
    private Vector3 lastPos;

    void Update()
    {
        bool triggerDown = Input.GetButtonDown(grabInputName);
        bool triggerUp   = Input.GetButtonUp(grabInputName);

        if (triggerDown)
        {
            TryGrab();
        }
        else if (triggerUp)
        {
            TryRelease();
        }

        lastPos = transform.position;
    }

    void TryGrab()
    {
        if (heldBall != null) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (var hit in hits)
        {
            var ball = hit.GetComponent<GrabbableBall>();
            if (ball)
            {
                heldBall = ball;
                ball.Grab(transform);
                break;
            }
        }
    }

    void TryRelease()
    {
        if (heldBall != null)
        {
            Vector3 throwVelocity = (transform.position - lastPos) / Time.deltaTime;
            heldBall.Release(throwVelocity);
            heldBall = null;
        }
    }
}
