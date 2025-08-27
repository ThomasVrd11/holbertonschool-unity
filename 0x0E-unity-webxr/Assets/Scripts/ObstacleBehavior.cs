using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    public GameObject ExplosionEffect;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

	void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.CompareTag("Interactable"))
        {
            ExplosionEffect.SetActive(true);
            collision.gameObject.GetComponent<GrabbableBall>().ResetPosition();
            if (gameManager != null)
            {
                gameManager.ExitingLaneDelayedClear();
            }
        }
	}
}
