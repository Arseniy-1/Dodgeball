using Project.Scripts.Services;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;
using Project.Scripts.Entities;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerDodgeReadyState : EntityDodgeReadyState
    {
        private readonly PlayerInputController _playerInputController;
    
        public PlayerDodgeReadyState(Player player, AnimatorController animatorController, Ball ball, Mover mover, Collider squadZone, 
            Rigidbody rigidbody, PlayerConfig playerConfig, PlayerInputController playerInputController)
            : base(player, animatorController, ball, mover, squadZone, rigidbody, playerConfig)
        {
            _playerInputController = playerInputController;
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

        private void Dodge()
        {
            StateSwitcher.SwitchState<PlayerDodgeState>();
        }

        protected override void HandleBallZoneChanged(Collider zone)
        {
            if(GameStatusService.Instance.IsBallFree == false)
                return;
        
            if (zone == SquadZone)
                StateSwitcher.SwitchState<PlayerMoveState>();
        }
    }
}