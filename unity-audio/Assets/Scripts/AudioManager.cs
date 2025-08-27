using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    void Start()
    {
        float dbVolume = Mathf.Log10(Mathf.Clamp(SharedInfo.Instance.BGMVolume, 0.0001f, 1f)) * 20f;
        _audioMixer.SetFloat("BGMVolume", dbVolume);

    }


}
