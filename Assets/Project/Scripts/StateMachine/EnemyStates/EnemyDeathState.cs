using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyDeathState : EntityDeathState
    {
        public EnemyDeathState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
        }
    }
}