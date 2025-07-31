using Project.Scripts.Entities;
using Project.Scripts.Messages;
using UniRx;

namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityPrepareState : IState
    {
        private readonly StateDataHolder _stateDataHolder;
    
        private CompositeDisposable _disposable;

        protected IStateSwitcher StateSwitcher;

        protected EntityPrepareState(StateDataHolder dataHolder)
        {
            _stateDataHolder = dataHolder;
        }

        public void Initialize(IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        public virtual void Enter()
        {
            _disposable = new CompositeDisposable();

            MessageBrokerHolder.GameActions
                .Receive<M_GameStarted>()
                .Subscribe(_ => HandleStartGame())
                .AddTo(_disposable);

            _stateDataHolder.AnimatorController.PrepareToBattle();
            LookToTarget();
        }

        public virtual void Exit()
        {
            _disposable.Dispose();
        }

        public virtual void Update()
        {
        }

        protected abstract void HandleStartGame();
    
        private void LookToTarget()
        {
            Entity target = _stateDataHolder.TargetScanner.Scan(_stateDataHolder.Teammates);
        
            if (target == null) 
                return;

            _stateDataHolder.Rotator.RotateToTarget(target.transform, _stateDataHolder.Entity.transform);
        }
    }
}