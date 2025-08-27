using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _ammoPrefab;
    private int _score = 0;
    private int _ammoCount = 7;

    void Start()
    {
        _score = 0;
        _ammoCount = 7;
        EventManager.Instance.SetGameManager(this);
        EventManager.Instance.OnStartGame += SpawnAmmo;
        EventManager.Instance.AmmoLaunched += DecrementAmmoCount;
        EventManager.Instance.Scored += IncrementScore;
        EventManager.Instance.ShowRetry += CheckHighScore;
    }
    void OnDestroy()
    {
        EventManager.Instance.OnStartGame -= SpawnAmmo;
        EventManager.Instance.AmmoLaunched -= DecrementAmmoCount;
        EventManager.Instance.Scored -= IncrementScore;
        EventManager.Instance.ShowRetry -= CheckHighScore;

    }

    #region Actions
    /// <summary>
    /// Spawns ammo at the center of the screen in world coordinates.
    /// </summary>
    public void SpawnAmmo()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0.5f);
        Vector3 spawnWorldPos = Camera.main.ScreenToWorldPoint(screenCenter);
        GameObject ammo = Instantiate(_ammoPrefab, spawnWorldPos, Quaternion.identity);
        Debugger.ShowText("Ammo spawned at: " + Camera.main.transform.position);
        ammo.GetComponent<SlingshotAmmo>().Spawn();
    }
    /// <summary>
    /// Increments the score by 10 points and updates the score display through the EventManager.
    /// </summary>
    public void IncrementScore()
    {
        _score += 10;
        EventManager.Instance.AudioManager.PlayTargetHit();
        EventManager.Instance.UpdateScore();
    }

    /// <summary>
    /// Decrements the ammo count by 1 and updates the ammo count display through the EventManager.
    /// </summary>
    public void DecrementAmmoCount()
    {
        //Debugger.ShowText("Ammo count before decrement: " + _ammoCount);
        _ammoCount--;
        EventManager.Instance.AudioManager.PlayAmmoLaunched();
        //Debugger.AppendText("Ammo count decremented: " + _ammoCount);
        EventManager.Instance.UpdateAmmoCount(_ammoCount);
        //Debugger.ShowText("Ammo count decremented: " + _ammoCount);
    }

    /// <summary>
    /// Checks if the current score is higher than the stored high score in PlayerPrefs and updates it if necessary.
    /// </summary>
    private void CheckHighScore()
    {
        int high1 = PlayerPrefs.GetInt("HighScore1", 0);
        int high2 = PlayerPrefs.GetInt("HighScore2", 0);
        int high3 = PlayerPrefs.GetInt("HighScore3", 0);
        switch (_score)
        {
            case int n when n > high1:
                PlayerPrefs.SetInt("HighScore3", high2);
                PlayerPrefs.SetInt("HighScore2", high1);
                PlayerPrefs.SetInt("HighScore1", _score);
                break;
            case int n when n > high2:
                PlayerPrefs.SetInt("HighScore3", high2);
                PlayerPrefs.SetInt("HighScore2", _score);
                break;
            case int n when n > high3:
                PlayerPrefs.SetInt("HighScore3", _score);
                break;
            default:
                break;
        }
        PlayerPrefs.Save();
        EventManager.Instance.TriggerRefreshHighScore();

    }
    #endregion
    #region Getters
    /// <summary>
    /// Returns the current ammo count of the game.
    /// </summary>
    /// <returns></returns>
    public int GetAmmoCount()
    {
        return _ammoCount;
    }
    /// <summary>
    /// Returns the current score of the game.
    /// </summary>
    /// <returns></returns>
    public int GetScore()
    {
        return _score;
    }
    #endregion
    #region Buttons Behaviour
    /// <summary>
    /// Restarts the game by resetting the score and ammo count, and triggering the reset game event.
    /// </summary>
    public void RestartGame()
    {
        _score = 0;
        _ammoCount = 7;
        EventManager.Instance.ResetGame();
    }

    /// <summary>
    /// Reloads the current game scene, effectively restarting the game.
    /// </summary>
    public void ReloadGame()
    {
        ARPlaneSelector.SelectedPlane = null;
        Reloading.SceneToLoad = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Reload");
    }
    /// <summary>
    /// Quits the game application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    public void StartGame()
    {
        EventManager.Instance.TriggerStartGame();
    }
}
