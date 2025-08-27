using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [Tooltip("0-ammo launched, 1-target hit, 2-target miss, 3-game start, 4-game over, 5-button pressed")]
    [SerializeField] private AudioClip[] _audioClips;
    void Start()
    {
        EventManager.Instance.SetAudioManager(this);
    }
    public void PlayAmmoLaunched()
    {
        if (_audioClips.Length > 0 && _audioClips[0] != null)
        {
            _audioSource.PlayOneShot(_audioClips[0]);
        }
    }
    public void PlayTargetHit()
    {
        if (_audioClips.Length > 1 && _audioClips[1] != null)
        {
            _audioSource.PlayOneShot(_audioClips[1]);
        }
    }

    public void PlayTargetMiss()
    {
        if (_audioClips.Length > 2 && _audioClips[2] != null)
        {
            _audioSource.PlayOneShot(_audioClips[2]);
        }
    }
    public void PlayGameStart()
    {
        if (_audioClips.Length > 3 && _audioClips[3] != null)
        {
            _audioSource.PlayOneShot(_audioClips[3]);
        }
    }
    public void PlayGameOver()
    {
        if (_audioClips.Length > 4 && _audioClips[4] != null)
        {
            _audioSource.PlayOneShot(_audioClips[4]);
        }
    }
    public void PlayButtonPressed()
    {
        if (_audioClips.Length > 5 && _audioClips[5] != null)
        {
            _audioSource.PlayOneShot(_audioClips[5]);
        }
    }
}
