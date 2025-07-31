using Project.Scripts.Services;
using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerCelebrateState : EntityCelebrateState
    {
        private readonly PlayerInputController _playerInputController;
    
        public PlayerCelebrateState(StateDataHolder dataHolder, PlayerInputController playerInputController) 
            : base(dataHolder)
        {
            _playerInputController = playerInputController;
        }

        public override void Enter()
        {
            base.Enter();
            _playerInputController.enabled = false;
        }

        public override void Exit()
        {
            base.Exit();
            _playerInputController.enabled = true;
        }
    }
}