using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerIdleState : EntityIdleState
    {
        public PlayerIdleState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
        }

        protected override void SwitchToMove()
        {
            StateSwitcher.SwitchState<PlayerMoveState>();
        }

        protected override void SwitchToDodge()
        {
            StateSwitcher.SwitchState<PlayerDodgeReadyState>();
        }
    }
}