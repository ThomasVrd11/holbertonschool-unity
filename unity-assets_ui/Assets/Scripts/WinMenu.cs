using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    int currentSceneIndex;
    private void Start() {
        currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
    }
    public void MainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void Next()
    {
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }
}
