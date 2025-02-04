using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Public variable for the player GameObject, editable in the Inspector
    public GameObject player;

    // Private variable to store the offset distance between the player and the camera
    private Vector3 offset;

    void Start()
    {
        // Calculate and store the offset value by finding the difference between the camera's position and the player's position
        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }
        else
        {
            Debug.LogError("Player GameObject not assigned to CameraFollow script in the Inspector!");
        }
    }

    void LateUpdate()
    {
        // If the player is assigned, update the camera's position to maintain the offset
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
