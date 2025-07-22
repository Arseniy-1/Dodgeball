using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using Project.Scripts.Services.Ball;
using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerCelebrateState : EntityCelebrateState
    {
        private readonly PlayerInputController _inputController;
    
        public PlayerCelebrateState(
            Player player,
            AnimatorController animatorController,
            BallHolder ballHolder,
            BallThrower ballThrower,
            CollisionHandler collisionHandler,
            PlayerInputController playerInputController,
            List<Entity> teammates)
            : base(
                player,
                animatorController,
                ballHolder,
                ballThrower,
                collisionHandler,
                teammates)
        {
            _inputController = playerInputController;
        }

        public override void Enter()
        {
            base.Enter();
            _inputController.enabled = false;
        }

        public override void Exit()
        {
            base.Exit();
            _inputController.enabled = true;
        }
    }
}