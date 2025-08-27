using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    public bool isInverted = false;

    [SerializeField] private float _rotationSpeed = 3f;
    private Vector3 _offset;
    private float _yaw;
    private float _pitch;
    

    private void Start()
    {
        _offset = Player.transform.position - this.transform.position;
        _yaw = transform.eulerAngles.y;
        float rawPitch = transform.eulerAngles.x;
        _pitch = (rawPitch > 180) ? rawPitch - 360 : rawPitch;
        if (SharedInfo.Instance.InvertY)
        {
            isInverted = true;
        }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        HandleCameraRotation();
        FollowPlayer();
    }

    private void HandleCameraRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * _rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * _rotationSpeed;
            _yaw += mouseX;
            _pitch += isInverted? mouseY : -mouseY;
            _pitch = Mathf.Clamp(_pitch, -15f, 60f);

            Quaternion cameraRotation = Quaternion.Euler(_pitch, _yaw, 0f);
            Vector3 rotatedOffset = cameraRotation * _offset;

            transform.position = Player.transform.position - rotatedOffset;
            transform.LookAt(Player.transform.position);
        }
    }
    private void FollowPlayer()
    {
        if (!Input.GetMouseButton(1))
        {
            transform.position = Player.transform.position - Quaternion.Euler(_pitch, _yaw, 0f) * _offset;
            transform.LookAt(Player.transform.position);
        }
    }
}
