using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{

    [SerializeField] private Toggle _invertYToggle;
    private bool _invertYDefault;
    private void Start() {
        
        _invertYDefault = _invertYToggle.isOn = SharedInfo.Instance.InvertY;
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
}
