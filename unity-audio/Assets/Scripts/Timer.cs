using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public Text TimerText;

    [SerializeField] private GameObject _timerPanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private TMP_Text _winText;
    private float _time = 0f;
    private bool _timerIsStopped = false;

    // Update is called once per frame
    void Update()
    {
        if (!_timerIsStopped)
            _time += Time.deltaTime;
    }
    private void LateUpdate()
    {
        if (!_timerIsStopped)
            UpdateTimerDisplay();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(_time / 60f);
        float seconds = _time % 60f;
        int milliseconds = Mathf.FloorToInt((_time * 100f) % 100f);

        TimerText.text = string.Format("{0:0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void Win()
    {
        _timerIsStopped = true;
        _winPanel.SetActive(true);
        _timerPanel.SetActive(false);
        _winText.text = TimerText.text;
    }

}
