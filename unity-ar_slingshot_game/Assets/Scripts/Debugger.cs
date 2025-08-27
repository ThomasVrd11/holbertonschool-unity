using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    public static Debugger Instance { get; private set; }
    public bool isDebugEnabled = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        ClearText();
    }
    /// <summary>
    /// Shows a debug message in the UI text element if debugging is enabled.
    /// </summary>
    /// <param name="message"></param>
    public static void ShowText(string message)
    {
        if (Instance != null && Instance.debugText != null && Instance.isDebugEnabled)
        {
            Instance.debugText.text = message;
            Instance.debugText.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Appends a debug message to the existing text in the UI text element if debugging is enabled.
    /// </summary>
    /// <param name="message"></param>
    public static void AppendText(string message)
    {
        if (Instance != null && Instance.debugText != null && Instance.isDebugEnabled)
        {
            Instance.debugText.text += "\n" + message;
        }
    }

    /// <summary>
    /// Clears the debug text in the UI text element if debugging is enabled.
    /// </summary>
    public static void ClearText()
    {
        if (Instance != null && Instance.debugText != null && Instance.isDebugEnabled)
        {
            Instance.debugText.text = "";
        }
    }
}
