namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityCelebrateState : IState
    {
        private readonly StateDataHolder _stateDataHolder;

        protected EntityCelebrateState(StateDataHolder dataHolder)
        {
            _stateDataHolder = dataHolder;
        }

        public virtual void Enter()
        {
            _stateDataHolder.BallHolder.LostBall();
            _stateDataHolder.BallHolder.enabled = false;
            _stateDataHolder.BallThrower.enabled = false;
            _stateDataHolder.CollisionHandler.enabled = false;

            _stateDataHolder.AnimatorController.Celebrate();
        }

        public virtual void Exit()
        {
            _stateDataHolder.BallHolder.enabled = true;
            _stateDataHolder.BallThrower.enabled = true;
            _stateDataHolder.CollisionHandler.enabled = true;
        }

        public void Initialize(IStateSwitcher stateSwitcher)
        {
        }

        public virtual void Update()
        {
        }
    }
}