using System.Collections.Generic;
using Project.Scripts.Messages;
using Project.Scripts.Services;
using Project.Scripts.Entities;
using UniRx;

namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityPrepareState : IState
    {
        private readonly AnimatorController _animatorController;
        private readonly Entity _entity;
        private readonly List<Entity> _teammates;
        private readonly TargetScanner _targetScanner;
        private readonly Rotator _rotator;
    
        private CompositeDisposable _disposable;

        protected IStateSwitcher StateSwitcher;

        protected EntityPrepareState(Entity entity, AnimatorController animatorController, TargetScanner targetScanner, List<Entity> teammates)
        {
            _entity = entity;
            _animatorController = animatorController;
            _targetScanner = targetScanner;
            _teammates = teammates;
            _rotator = new Rotator();
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

            _animatorController.PrepareToBattle();
            LookToTarget();
        }

        public virtual void Exit()
        {
            _disposable.Dispose();
        }

        public virtual void Update() { }

        protected abstract void HandleStartGame();
    
        private void LookToTarget()
        {
            Entity target = _targetScanner.Scan(_teammates);
        
            if (target == null) 
                return;

            _rotator.RotateToTarget(target.transform, _entity.transform);
        }
    }
}