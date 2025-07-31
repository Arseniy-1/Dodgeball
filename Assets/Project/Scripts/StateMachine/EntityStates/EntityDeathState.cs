namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityDeathState : IState
    {
        private readonly StateDataHolder _stateDataHolder;

        protected EntityDeathState(StateDataHolder dataHolder)
        {
            _stateDataHolder = dataHolder;
        }

        public virtual void Enter()
        {
            _stateDataHolder.AnimatorController.Death();
        
            _stateDataHolder.BallThrower.StopCharging();
            _stateDataHolder.BallHolder.LostBall();
            _stateDataHolder.CollisionHandler.enabled = false;
            _stateDataHolder.Collider.enabled = false;
        }

        public virtual void Exit()
        {
            _stateDataHolder.CollisionHandler.enabled = true;
            _stateDataHolder.Collider.enabled = true;
        }
    
        public void Initialize(IStateSwitcher stateSwitcher)
        {
        }

        public virtual void Update()
        {
        }
    }
}