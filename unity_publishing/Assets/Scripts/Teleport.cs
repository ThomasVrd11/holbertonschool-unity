using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private Teleport destination;

    private bool isTeleporting = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            if (destination != null)
            {
                
                destination.isTeleporting = true;
                other.transform.position = destination.transform.position;

                Debug.Log("Player teleported to: " + destination.transform.position);
            }
            else
            {
                Debug.LogWarning("Paired Teleporter is not assigned!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTeleporting = false;
        }
    }
}
