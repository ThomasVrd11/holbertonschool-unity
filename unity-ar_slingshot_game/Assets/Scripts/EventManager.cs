using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    public event Action OnStartGame;
    public event Action AmmoLaunched;
    public event Action Scored;
    public event Action OnScoreUpdated;
    public event Action ShowRetry;
    public event Action RefreshHighscore;
    public event Action OnResetGame;

    public event Action<int> OnAmmoCountUpdated;

    public GameManager GameManager { get; private set; }
    public AudioManager AudioManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #region Events callbacks

    /// <summary>
    /// Triggers the start game event.
    /// </summary>
    public void TriggerStartGame()
    {
        Debugger.ShowText("Game started");
        OnStartGame?.Invoke();
        AudioManager.PlayGameStart();
    }

    /// <summary>
    /// Triggers the ammo launched event.
    /// </summary>
    public void TriggerAmmoLaunched()
    {
        AmmoLaunched?.Invoke();
    }
    /// <summary>
    /// Triggers the scored event when a target is hit.
    /// </summary>
    public void TriggerScored()
    {
        Scored?.Invoke();
    }
    /// <summary>
    /// Sets the GameManager instance for the EventManager.
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetGameManager(GameManager gameManager)
    {
        GameManager = gameManager;
    }
    /// <summary>
    /// Sets the AudioManager instance for the EventManager.
    /// </summary>
    /// <param name="audioManager"></param>
    public void SetAudioManager(AudioManager audioManager)
    {
        AudioManager = audioManager;
    }
    /// <summary>
    /// Updates the score and triggers the OnScoreUpdated event.
    /// </summary>
    public void UpdateScore()
    {
        OnScoreUpdated?.Invoke();
    }
    /// <summary>
    /// Updates the ammo count and triggers the OnAmmoCountUpdated event.
    /// </summary>
    /// <param name="ammoNbr"></param>
    public void UpdateAmmoCount(int ammoNbr)
    {
        OnAmmoCountUpdated?.Invoke(ammoNbr);
    }
    /// <summary>
    /// Triggers the ShowRetry event to display the retry UI when the game ends.
    /// </summary>
    public void TriggerRefreshHighScore()
    {
        RefreshHighscore?.Invoke();
    }
    /// <summary>
    /// Triggers the OnResetGame event to display the retry UI when the game ends.
    /// </summary>
    public void ResetGame()
    {
        OnResetGame?.Invoke();
        AudioManager.PlayGameStart();
    }
    /// <summary>
    /// Checks if the game has ended due to running out of ammo.
    /// </summary>
    public void CheckEndGame()
    {
        if (GameManager.GetAmmoCount() <= 0)
        {
            AudioManager.PlayGameOver();
            ShowRetry?.Invoke();
        }
    }
    #endregion
}
