using UnityEngine;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameObject _TimerCanvas;

    public void EnableStartGame()
    {
        _mainCamera.SetActive(true);
        _playerController.enabled = true;
        _TimerCanvas.SetActive(true);
        this.enabled = false;
        this.gameObject.SetActive(false);
    }
}
