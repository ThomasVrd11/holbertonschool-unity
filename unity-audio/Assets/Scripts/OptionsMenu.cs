using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField] private Toggle _invertYToggle;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private AudioMixer _audioMixer;
    private bool _invertYDefault;
    private void Start() {
        
        _invertYDefault = _invertYToggle.isOn = SharedInfo.Instance.InvertY;
        _bgmSlider.value = SharedInfo.Instance.BGMVolume;
        _bgmSlider.onValueChanged.AddListener(updateBGMVolume);
        _sfxSlider.value = SharedInfo.Instance.SFXVolume;
        _sfxSlider.onValueChanged.AddListener(updateSFXVolume);
    }

    public void Back()
    {
        SceneManager.LoadScene(SharedInfo.Instance.PreviousScene);
    }

    public void Apply()
    {
        if(_invertYToggle.isOn != _invertYDefault) {
            SharedInfo.Instance.SetInvertY(_invertYToggle.isOn);
        }
        Back();
    }

    private void updateBGMVolume(float value)
    {
        SharedInfo.Instance.BGMVolume = value;
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        _audioMixer.SetFloat("BGMVolume", volume);
    }
    private void updateSFXVolume(float value)
    {
        SharedInfo.Instance.SFXVolume = value;
        float volume = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        _audioMixer.SetFloat("SFXVolume", volume);
    }
}
