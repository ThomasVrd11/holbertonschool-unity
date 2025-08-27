using UnityEngine;
using UnityEngine.InputSystem;

public class TouchInputReader : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    public Vector2 TouchPosition { get; private set; }
    public bool IsTouching { get; private set; }

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Touch.Enable();
        _inputActions.Touch.TouchPosition.performed += OnTouchPosition;
    }

    private void OnDisable()
    {
        _inputActions.Touch.TouchPosition.performed -= OnTouchPosition;
        _inputActions.Touch.Disable();
    }

    private void OnTouchPosition(InputAction.CallbackContext context)
    {
        TouchPosition = context.ReadValue<Vector2>();
        IsTouching = true;
    }

    public void ResetTouch()
    {
        IsTouching = false;
    }
}
