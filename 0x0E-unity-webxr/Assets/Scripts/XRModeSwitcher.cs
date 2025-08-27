using UnityEngine;

public class XRModeSwitcher : MonoBehaviour
{
    public enum XRMode { Normal, VR, AR, Unknown }

    [SerializeField] private MonoBehaviour _webXRManager;

    private System.Type _managerType;
    private object _managerInstance;

    public XRMode CurrentMode { get; private set; } = XRMode.Unknown;

    void Awake()
    {
        if (_webXRManager == null)
        {
            Debug.LogWarning("No WebXRManager assigned to XRModeSwitcher.");
            return;
        }

        _managerInstance = _webXRManager;
        _managerType = _managerInstance.GetType();

        var prop = _managerType.GetProperty("XRState");
        if (prop == null)
        {
            Debug.LogWarning("WebXRManager.XRState property not found.");
            return;
        }

        string state = prop.GetValue(_managerInstance)?.ToString();
        CurrentMode = state switch
        {
            "NORMAL" => XRMode.Normal,
            "VR"     => XRMode.VR,
            "AR"     => XRMode.AR,
            _        => XRMode.Unknown
        };

        Debug.Log($"[XRModeSwitcher] Detected XR Mode: {CurrentMode}");
    }
}
