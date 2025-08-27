using UnityEngine;

public class Deadzone : MonoBehaviour
{
    public GameManager gameManager;
    public float requiredTime = 8f;
    public string targetTag = "Interactable";

    private float stayTimer = 0f;
    private bool triggered = false;

    void OnTriggerStay(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag(targetTag)) return;

        stayTimer += Time.deltaTime;

        if (stayTimer >= requiredTime)
        {
            triggered = true;
            OnStayComplete(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        stayTimer = 0f;
        triggered = false;
    }

    private void OnStayComplete(GameObject obj)
    {
        obj.GetComponent<GrabbableBall>()?.ResetPosition();
        gameManager.ExitingLane();
    }
}
