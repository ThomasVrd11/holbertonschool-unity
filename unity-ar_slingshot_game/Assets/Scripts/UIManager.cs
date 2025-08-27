using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreTxt;
    [SerializeField] private TMP_Text _highscoreTxt;
    [SerializeField] private GameObject _retryPanel;
    [SerializeField] private GameObject _gameUI;
    
    [SerializeField] private AmmoSelected[] _ammoCounterTokens;

    private void Start()
    {
        if (_retryPanel.activeSelf)
            _retryPanel.SetActive(false);
        if (_gameUI.activeSelf)
            _gameUI.SetActive(false);
        EventManager.Instance.OnScoreUpdated += UpdateScoreText;
        EventManager.Instance.OnAmmoCountUpdated += UpdateAmmoCounter;
        EventManager.Instance.OnResetGame += ResetGame;
        EventManager.Instance.ShowRetry += ShowRetryButton;
        SetHighScore();

    }

    private void OnDestroy()
    {
        EventManager.Instance.OnScoreUpdated -= UpdateScoreText;
        EventManager.Instance.OnAmmoCountUpdated -= UpdateAmmoCounter;
        EventManager.Instance.OnResetGame -= ResetGame;
        EventManager.Instance.ShowRetry -= ShowRetryButton;
    }
    /// <summary>
    /// Updates the score text in the UI to reflect the current score from the GameManager.
    /// </summary>
    private void UpdateScoreText()
    {
        if (_scoreTxt != null)
        {
            _scoreTxt.text = $"Score: {EventManager.Instance.GameManager.GetScore()}";
        }
    }
    /// <summary>
    /// Updates the ammo counter in the UI based on the current ammo number.
    /// </summary>
    /// <param name="ammoNbr"></param>
    private void UpdateAmmoCounter(int ammoNbr)
    {

        try
        {

            if (_ammoCounterTokens == null || _ammoCounterTokens.Length == 0) return;
            int highlightIndex = ammoNbr - 1;
            int usedIndex = ammoNbr;

            Debugger.AppendText($"Ammo index {ammoNbr} selected.");
            if (usedIndex >= 0 && usedIndex < _ammoCounterTokens.Length)
            {
                _ammoCounterTokens[usedIndex].UnselectAmmo();
                _ammoCounterTokens[usedIndex].SetUsedAmmo();
            }
            if (highlightIndex >= 0 && highlightIndex < _ammoCounterTokens.Length)
            {
                _ammoCounterTokens[highlightIndex].SetSelectedAmmo();
            }
            else
            {
                Debugger.ShowText($"No more ammo to highlight (ammoNbr: {ammoNbr})");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error updating ammo counter: {ex.Message}");
        }
    }
    /// <summary>
    /// Resets the game by resetting the ammo counter, updating the score text, and hiding the retry button.
    /// </summary>
    private void ResetGame()
    {
        ResetAmmoCounter();
        UpdateScoreText();
        _retryPanel.SetActive(false);
    }

    /// <summary>
    /// Resets the ammo counter by unselecting all ammo tokens and resetting their used state.
    /// </summary>
    private void ResetAmmoCounter()
    {
        foreach (var ammoToken in _ammoCounterTokens)
        {
            ammoToken.UnselectAmmo();
            ammoToken.ResetUsedAmmo();
        }
        _ammoCounterTokens[_ammoCounterTokens.Length - 1].SetSelectedAmmo();
    }

    /// <summary>
    /// Displays the retry button in the UI, allowing the player to restart the game.
    /// </summary>
    private void ShowRetryButton()
    {
        _retryPanel.SetActive(true);
        SetHighScore();
    }

    /// <summary>
    /// Sets the high score text in the UI to reflect the provided high score value.
    /// </summary>
    public void SetHighScore()
    {
        if (_highscoreTxt != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore1", 0);
            _highscoreTxt.text = $"{highScore}";
        }
    }

}
