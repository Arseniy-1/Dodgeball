using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyDodgeState : EntityDodgeState
    {
        public EnemyDodgeState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
        }

        protected override void OnJumpFinished()
        {
            StateSwitcher.SwitchState<EnemyDodgeReadyState>();
        }
    }
}