using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float JumpForce = 5f;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private float _groundCheckRadius = 0.3f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private TrailRenderer _trailRenderer;
    private float _moveX;
    private float _moveZ;
    private bool _isGrounded;
    private bool _jumpPressed;

    void Update()
    {
        GetInputs();
        RespawnIfFallen();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
        if (_isGrounded && _trailRenderer.emitting == true)
        {
            _trailRenderer.emitting = false;
        }

        HandleMovement();
        HandleJump();
    }

    private void GetInputs()
    {
        _moveX = Input.GetAxis("Horizontal");
        _moveZ = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _jumpPressed = true;
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = new Vector3(_moveX, 0f, _moveZ).normalized;
        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 move = transform.right * _moveX + transform.forward * _moveZ;
            Vector3 newPosition = move * MoveSpeed * Time.fixedDeltaTime;

            _rb.MovePosition(_rb.position + newPosition);
        }
    }

    private void HandleJump()
    {
        if (_jumpPressed)
        {
            _trailRenderer.emitting = true;
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            _jumpPressed = false;
        }
    }
    /*private void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }*/

    private void RespawnIfFallen()
    {
        if (transform.position.y < -15)
        {
            transform.position = _respawnPoint.position;
        }
    }
}
