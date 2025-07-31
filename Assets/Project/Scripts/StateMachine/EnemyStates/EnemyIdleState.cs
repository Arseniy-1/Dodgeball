using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyIdleState : EntityIdleState
    {
        public EnemyIdleState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
        }

        protected override void SwitchToMove()
        {
            StateSwitcher.SwitchState<EnemyMoveState>();
        }

        protected override void SwitchToDodge()
        {
            StateSwitcher.SwitchState<EnemyDodgeReadyState>();
        }
    }
}