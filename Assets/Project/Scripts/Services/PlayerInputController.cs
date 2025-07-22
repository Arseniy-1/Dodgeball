using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Services
{
    public class PlayerInputController : MonoBehaviour
    {
        private PlayerInput _payerInput;

        public event Action ActionButtonStarted;
        public event Action ActionButtonCanceled;

        private void Awake()
        {
            _payerInput = new PlayerInput();
            _payerInput.Enable();
        }

        private void OnEnable()
        {
            _payerInput.Player.Action.started += OnActionButtonStarted;
            _payerInput.Player.Action.canceled += OnActionButtonCanceled;
        }

        private void OnDisable()
        {
            _payerInput.Player.Action.started -= OnActionButtonStarted;
            _payerInput.Player.Action.canceled -= OnActionButtonCanceled;
        }

        [Button]
        private void OnActionButtonStarted(InputAction.CallbackContext _)
        {
            ActionButtonStarted?.Invoke();
        }

        [Button]
        private void OnActionButtonCanceled(InputAction.CallbackContext _)
        {
            ActionButtonCanceled?.Invoke();
        }
    }
}