using UnityEngine;

public class SharedInfo : MonoBehaviour
{
    private static SharedInfo _instance;
    public static SharedInfo Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<SharedInfo>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject("SharedInfo");
                    _instance = obj.AddComponent<SharedInfo>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    public string PreviousScene { get; private set; } = "MainMenu";
    public bool InvertY { get; set; } = false;

    [SerializeField] private float _bgmVolume = 1f;
    [SerializeField] private float _sfxVolume = 1f;
    public float BGMVolume
    {
        get => _bgmVolume;
        set
        {
            _bgmVolume = Mathf.Clamp01(value);
            PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
            PlayerPrefs.Save();
        }
    }
    public float SFXVolume
        {
            get => _sfxVolume;
            set
            {
                _sfxVolume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
                PlayerPrefs.Save();
            }
        }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        _bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    public void SetPreviousScene(string scene)
    {
        PreviousScene = scene;
    }

    public void SetInvertY(bool invert)
    {
        InvertY = invert;
    }
}
