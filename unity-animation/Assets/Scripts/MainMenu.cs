using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    public void LevelSelect(int level)
    {
        SharedInfo.Instance.SetPreviousScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(level);
    }

    public void Options()
    {
        SharedInfo.Instance.SetPreviousScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Options");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        Debug.Log("Exited");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
