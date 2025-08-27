using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WebXRMovementManager movementManager;
    public CameraSwitch cameraSwitch;
    public LaneObstacleSpawner laneObstacleSpawner;
    public TMP_Text scoreText;
    private int score = 0;
    void OnEnable()
    {
        Pin.PinFallen += OnPinFallen;
    }

    void OnDisable()
    {
        Pin.PinFallen -= OnPinFallen;
    }
    public void EnteringLane(Rigidbody ballRb)
    {
        movementManager.EnterBallSteering(ballRb);
        cameraSwitch.SwitchCam(true);
        laneObstacleSpawner.SpawnObstacles();
    }
    public void ExitingLane()
    {
        movementManager.ExitBallSteering();
        cameraSwitch.SwitchCam(false);
        laneObstacleSpawner.ClearObstacles();
    }
    public void ExitingLaneDelayedClear()
    {
        movementManager.ExitBallSteering();
        cameraSwitch.SwitchCam(false);
        laneObstacleSpawner.ClearObstaclesDelayed();
    }
    private void OnPinFallen(Pin fallenPin)
    {
        score++;
        scoreText.text = score.ToString();
    }
    
}
