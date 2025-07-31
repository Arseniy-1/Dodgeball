using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerDodgeState : EntityDodgeState
    {
        public PlayerDodgeState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
        }

        protected override void OnJumpFinished()
        {
            StateSwitcher.SwitchState<PlayerDodgeReadyState>();
        }
    }
}