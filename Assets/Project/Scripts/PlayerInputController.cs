using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerInput _payerInput;

    public event Action ActionButtonPressed;
    public event Action ActionButtonStarted;
    public event Action ActionButtonCanceled;

    private void Awake()
    {
        _payerInput = new PlayerInput();
        _payerInput.Enable();
    }

    private void OnEnable()
    {
        _payerInput.Player.Action.performed += OnActionButtonPerformed;
        _payerInput.Player.Action.started += OnActionButtonStarted;
        _payerInput.Player.Action.canceled += OnActionButtonCanceled;
    }

    private void OnDisable()
    {
        _payerInput.Player.Action.performed -= OnActionButtonPerformed;
        _payerInput.Player.Action.started -= OnActionButtonStarted;
        _payerInput.Player.Action.canceled -= OnActionButtonCanceled;
    }

    private void Update()
    {
        // if (_payerInput.Player.Action.IsPressed())
            // ActionButtonPressed?.Invoke();
    }

    private void OnActionButtonPerformed(InputAction.CallbackContext callbackContext)
    {
        ActionButtonPressed?.Invoke();
    }

    private void OnActionButtonStarted(InputAction.CallbackContext callbackContext)
    {
        ActionButtonStarted?.Invoke();
    }

    private void OnActionButtonCanceled(InputAction.CallbackContext callbackContext)
    {
        ActionButtonCanceled?.Invoke();
    }
}