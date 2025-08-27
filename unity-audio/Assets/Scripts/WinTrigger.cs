using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudiosource;
    [SerializeField] private AudioSource fanfareAudiosource;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Timer>().Win();
            bgmAudiosource.Stop();
            fanfareAudiosource.Play();
        }
    }
}
