using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float JumpForce = 5f;
    public float RotationSpeed = 10f;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private float _groundCheckRadius = 0.5f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private Animator _animator;

    private float _moveX;
    private float _moveZ;
    private bool _isGrounded;
    private bool _jumpPressed;
    private bool _isFalling = false;
    private bool _isStanding = true;

    public event System.Action ImpactSoundTriggered;

    public bool IsGrounded => _isGrounded;

    void Update()
    {
        GetInputs();
        RespawnIfFallen();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
        if (_isGrounded && _isFalling == true)
        {
            _animator.SetBool("isFalling", false);
            _isStanding = false;
        }
        if (_isGrounded && _trailRenderer.emitting == true)
        {
            _trailRenderer.emitting = false;
            _animator.SetBool("isJumping", false);
        }
        if (!_isFalling && _isStanding)
        {
            HandleMovement();
            HandleJump();
        }
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

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * _moveZ + camRight * _moveX; ;
        if (moveDirection.magnitude >= 0.1f)
        {
            _animator.SetBool("isRunning", true);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotationSpeed * Time.fixedDeltaTime);

            Vector3 newPosition = transform.forward * MoveSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + newPosition);
        }
        else
        {
            _animator.SetBool("isRunning", false);
        }
    }

    private void HandleJump()
    {
        if (_jumpPressed)
        {
            _trailRenderer.emitting = true;
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            _jumpPressed = false;
            _animator.SetBool("isJumping", true);
        }
    }
    private void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }

    private void RespawnIfFallen()
    {
        if (transform.position.y < -15)
        {
            _isFalling = true;
            _animator.SetBool("isFalling", true);
            transform.position = _respawnPoint.position;
        }
    }
    public void StandBackUp()
    {
        _isStanding = true;
        _isFalling = false;
    }

    public void TriggerImpactSound()
    {
        _animator.SetBool("isRunning", false);
        ImpactSoundTriggered?.Invoke();
    }
}
