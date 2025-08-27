using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] private AudioSource playerAudioSource;
    [SerializeField] private AudioSource impactAudioSource;

    [SerializeField] private AudioClip grassSteps;
    [SerializeField] private AudioClip rockSteps;


    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;


	void Start()
	{
		playerController.ImpactSoundTriggered += OnImpactSoundTriggered;
	}

	void OnDestroy()
	{
		playerController.ImpactSoundTriggered -= OnImpactSoundTriggered;
	}

	void Update()
    {
        bool isRunning = animator.GetBool("isRunning");
        bool isJumping = animator.GetBool("isJumping");
        bool isFalling = animator.GetBool("isFalling");



        if (isRunning && !isJumping && !isFalling && IsGrounded())
        {
            if (!playerAudioSource.isPlaying)
                playerAudioSource.Play();
        }
        else
        {
            if (playerAudioSource.isPlaying)
                playerAudioSource.Stop();
        }
    }

    private bool IsGrounded()
    {
        return playerController.IsGrounded;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock"))
            playerAudioSource.clip = rockSteps;

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Rock"))
            playerAudioSource.clip = grassSteps;
    }

    private void OnImpactSoundTriggered()
    {
        playerAudioSource.Stop();
        impactAudioSource.Play();
    }


}
