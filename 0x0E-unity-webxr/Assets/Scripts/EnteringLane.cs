using UnityEngine;

public class EnteringLane : MonoBehaviour
{
    public GameManager gameManager;
    public TriggerType triggerType;
    public enum TriggerType { Enter, Exit }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            GrabbableBall ballScript = other.gameObject.GetComponent<GrabbableBall>();
            Rigidbody ballRb = other.attachedRigidbody;
            if (triggerType == TriggerType.Enter)
            {
                ballScript.SetInPlay(true);
                gameManager.EnteringLane(ballRb);
            }
            else if (triggerType == TriggerType.Exit)
            {
                gameManager.ExitingLane();
                ballScript.SetInPlay(false);
                ballScript.StartResetPosition();
            }

        }
	}
}
