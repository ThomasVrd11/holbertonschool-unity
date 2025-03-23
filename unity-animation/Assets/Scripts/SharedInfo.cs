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

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
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
