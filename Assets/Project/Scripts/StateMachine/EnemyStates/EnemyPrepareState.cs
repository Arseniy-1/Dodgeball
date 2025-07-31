using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyPrepareState : EntityPrepareState
    {
        public EnemyPrepareState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
        }

        protected override void HandleStartGame()
        {
            StateSwitcher.SwitchState<EnemyIdleState>();
        }
    }
}