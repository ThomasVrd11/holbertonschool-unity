using System.Collections;
using UnityEngine;

public class GrabbableBall : MonoBehaviour
{
    private bool isGrabbed = false;
    private Transform grabber;
    private Rigidbody rb;
    private Vector3 originalPosition;
    private float delay = 4f;

    public float autoResetDelay = 8f;
    public float stillVelocityThreshold = 0.05f;

    private float stillTimer = 0f;
    public GameManager gameManager;
    private bool hasTriggeredExit = false;
    private bool inPlay = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (isGrabbed && grabber != null)
        {
            transform.position = grabber.position;
            transform.rotation = grabber.rotation;
            return;
        }
        if (rb.linearVelocity.magnitude < stillVelocityThreshold && inPlay)
        {
            stillTimer += Time.deltaTime;
            if (stillTimer >= autoResetDelay && !hasTriggeredExit)
            {
                Debug.Log("Ball stopped too long. Auto-resetting...");
                gameManager?.ExitingLane();
                ResetPosition();
                hasTriggeredExit = true;
                stillTimer = 0f;
            }
        }
        else
        {
            stillTimer = 0f;
        }
    }

    public void Grab(Transform hand)
    {
        isGrabbed = true;
        grabber = hand;
        rb.isKinematic = true;
        hasTriggeredExit = false;
    }

    public void Release(Vector3 throwVelocity)
    {
        isGrabbed = false;
        rb.isKinematic = false;
        grabber = null;

        StartCoroutine(ApplyImpulseNextFrame(throwVelocity));
    }

    private IEnumerator ApplyImpulseNextFrame(Vector3 throwVelocity)
    {
        yield return new WaitForFixedUpdate();

        Vector3 direction = transform.forward;

        if (throwVelocity.magnitude < 0.2f)
            throwVelocity = direction * 20f;

        rb.linearVelocity = throwVelocity;
        rb.AddForce(direction * 30f, ForceMode.Impulse);
    }
    public void StartResetPosition()
    {
        StartCoroutine(ResetPositionDelayed());
    }
    private IEnumerator ResetPositionDelayed()
    {
        yield return new WaitForSeconds(delay);
        ResetPosition();
    }

    public void ResetPosition()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = originalPosition;
        hasTriggeredExit = false;
        SetInPlay(false);
    }
    public void SetInPlay(bool value)
    {
        inPlay = value;
        Debug.Log("Ball in play: " + inPlay);
    }
}
