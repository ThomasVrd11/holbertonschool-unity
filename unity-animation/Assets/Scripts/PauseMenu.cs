using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Canvas pauseCanvas;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Pause(false);
            }
            else
            {
                Pause(true);
            }
        }
    }

    private void Pause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        //instruction unclear, hide canvas instead of gameobject
        //pauseScreen.SetActive(pause);
        pauseCanvas.enabled = pause;
    }

    public void Resume()
    {
        Pause(false);
    }

    public void Restart()
    {
        Pause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Pause(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void Options()
    {
        Pause(false);
        SharedInfo.Instance.SetPreviousScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("Options");
    }
}
