using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.PlayerStates
{
    public class PlayerDeathState : EntityDeathState
    {
        public PlayerDeathState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
        }
    }
}