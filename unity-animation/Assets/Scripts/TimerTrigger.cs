using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Timer>().enabled = true;
            this.gameObject.SetActive(false);
        }
    }
}
