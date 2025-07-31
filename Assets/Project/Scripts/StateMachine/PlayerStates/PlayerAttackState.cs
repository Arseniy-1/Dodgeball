using System;
using System.Threading.Tasks;
using Project.Scripts.Services;
using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerAttackState : EntityAttackState
    {
        private readonly PlayerInputController _playerInputController;

        private Action _buttonCanceledHandler;
        private Action _buttonStartedHandler;

        public PlayerAttackState(StateDataHolder dataHolder, PlayerInputController playerPlayerInputController) 
            : base(dataHolder)
        {
            _playerInputController = playerPlayerInputController;
        }

        public override async void Enter()
        {
            base.Enter();
            await ApplyTarget();

            _buttonStartedHandler = OnButtonClicked;
            _playerInputController.ActionButtonStarted += _buttonStartedHandler;
        }

        public override void Exit()
        {
            base.Exit();
            _playerInputController.ActionButtonStarted -= _buttonStartedHandler;
            _playerInputController.ActionButtonCanceled -= _buttonCanceledHandler;
        }

        private void OnButtonClicked()
        {
            StartAttack();

            _buttonCanceledHandler = () => _ = OnButtonReleasedAsync();
            _playerInputController.ActionButtonCanceled += _buttonCanceledHandler;
        }

        private async Task OnButtonReleasedAsync()
        {
            _playerInputController.ActionButtonStarted -= _buttonStartedHandler;
            _playerInputController.ActionButtonCanceled -= _buttonCanceledHandler;

            await ThrowBall();
            StateSwitcher.SwitchState<PlayerIdleState>();
        }
    }
}