using Project.Scripts.Services;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerDodgeReadyState : EntityDodgeReadyState
    {
        private readonly PlayerInputController _playerInputController;
        private readonly Collider _squadZone;
    
        public PlayerDodgeReadyState(StateDataHolder dataHolder, PlayerInputController playerInputController) 
            : base(dataHolder)
        {
            _playerInputController = playerInputController;
            _squadZone = dataHolder.SquadZone;
        }

        public override void Enter()
        {
            base.Enter();
            _playerInputController.ActionButtonStarted += Dodge;
        }

        public override void Exit()
        {
            base.Exit();
            _playerInputController.ActionButtonStarted -= Dodge;
        }

        protected override void HandleBallZoneChanged(Collider zone)
        {
            if(GameStatusService.Instance.IsBallFree == false)
                return;
        
            if (zone == _squadZone)
                StateSwitcher.SwitchState<PlayerMoveState>();
        }
     
        private void Dodge()
        {
            StateSwitcher.SwitchState<PlayerDodgeState>();
        }
    }
}