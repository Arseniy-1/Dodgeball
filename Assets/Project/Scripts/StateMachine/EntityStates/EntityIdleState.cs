using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityIdleState : IState
    {
        private readonly AnimatorController _animatorController;
        private readonly Ball _ball;
        private readonly Mover _mover;
        private readonly CollisionHandler _collisionHandler;
        private readonly Collider _collider;
        private readonly Rigidbody _rigidbody;
        private readonly AreaPointSelector _areaPointSelector;
        private readonly Rotator _rotator;
        private readonly Entity _entity;
        private readonly EntityConfig _entityConfig;
        private readonly Collider _squadZone;
        private readonly List<Entity> _teammates;

        private CancellationTokenSource _cancellationTokenSource;

        protected IStateSwitcher StateSwitcher;

        protected EntityIdleState(
            AnimatorController animatorController,
            Ball ball,
            Mover mover,
            CollisionHandler collisionHandler,
            Collider squadZone,
            Collider collider,
            Rigidbody rigidbody,
            Entity entity,
            EntityConfig entityConfig,
            List<Entity> teammates)
        {
            _animatorController = animatorController;
            _ball = ball;
            _mover = mover;
            _collisionHandler = collisionHandler;
            _squadZone = squadZone;
            _collider = collider;
            _rigidbody = rigidbody;
            _entity = entity;
            _entityConfig = entityConfig;
            _teammates = teammates;
            _areaPointSelector = new AreaPointSelector();
            _rotator = new Rotator();
        }

        public virtual void Initialize(IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        public virtual void Enter()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _rigidbody.isKinematic = true;
            _collisionHandler.enabled = false;
            _collider.isTrigger = true;

            _animatorController.Idle();
            RunIdleMovementLoop(_cancellationTokenSource.Token).Forget();

            HandleHolderChanged(_cancellationTokenSource.Token).Forget();
            TryMoveToBall(_cancellationTokenSource.Token).Forget();
        }

        public virtual void Exit()
        {
            _cancellationTokenSource.Cancel();

            _rigidbody.isKinematic = false;
            _collisionHandler.enabled = true;
            _collider.isTrigger = false;
        }

        public virtual void Update()
        {
            var ball = GameStatusService.Instance.CurrentBall;
            
            if (ball != null)
                _rotator.RotateToTarget(ball.transform, _entity.transform);
        }

        protected abstract void SwitchToMove();
        protected abstract void SwitchToDodge();

        private void HandleHolderChanged(Entity entity)
        {
            if (entity == null)
                return;

            if (_teammates.Contains(entity) == false)
                SwitchToDodge();
        }
    
        private async UniTaskVoid RunIdleMovementLoop(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                float standTime = Random.Range(_entityConfig.IdleMinStandTime, _entityConfig.IdleMaxStandTime);
                Vector3 target = _areaPointSelector.GetRandomPointInZone(_squadZone, _entity.transform.position);

                _animatorController.DodgeIdle();
                await _mover.MoveTo(target, _entityConfig.WalkSpeed, token);

                _animatorController.Idle();
                await UniTask.Delay(TimeSpan.FromSeconds(standTime), cancellationToken: token);
            }
        }

        private async UniTaskVoid HandleHolderChanged(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                await UniTask.WaitForFixedUpdate(cancellationToken: token);
                HandleHolderChanged(GameStatusService.Instance.CurrentHolder);
            }
        }

        private async UniTaskVoid TryMoveToBall(CancellationToken token)
        {
            float checkDelay = 0.3f;
        
            while (token.IsCancellationRequested == false)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(checkDelay), cancellationToken: token);

                GameStatusService.Instance.CurrentBall.Chargeable.OnCharged += HandleBallCharged;
            }
        }

        private void HandleBallCharged()
        {
            GameStatusService.Instance.CurrentBall.Chargeable.OnCharged -= HandleBallCharged;
        
            if (GameStatusService.Instance.IsBallFree)
            {
                if (GameStatusService.Instance.CurrentZone == _squadZone)
                {
                    SwitchToMove();
                }
            }
        }
    }
}