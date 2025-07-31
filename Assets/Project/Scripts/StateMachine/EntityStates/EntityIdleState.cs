using System;
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
        private readonly StateDataHolder _stateDataHolder;

        private CancellationTokenSource _cancellationTokenSource;

        protected IStateSwitcher StateSwitcher;

        protected EntityIdleState(StateDataHolder dataHolder)
        {
            _stateDataHolder = dataHolder;
        }

        public virtual void Initialize(IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        public virtual void Enter()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _stateDataHolder.Rigidbody.isKinematic = true;
            _stateDataHolder.CollisionHandler.enabled = false;
            _stateDataHolder.Collider.isTrigger = true;

            _stateDataHolder.AnimatorController.Idle();
            RunIdleMovementLoop(_cancellationTokenSource.Token).Forget();

            HandleHolderChanged(_cancellationTokenSource.Token).Forget();
            TryMoveToBall(_cancellationTokenSource.Token).Forget();
        }

        public virtual void Exit()
        {
            _cancellationTokenSource.Cancel();

            _stateDataHolder.Rigidbody.isKinematic = false;
            _stateDataHolder.CollisionHandler.enabled = true;
            _stateDataHolder.Collider.isTrigger = false;
        }

        public virtual void Update()
        {
            var ball = GameStatusService.Instance.CurrentBall;
            
            if (ball != null)
                _stateDataHolder.Rotator.RotateToTarget(ball.transform, _stateDataHolder.Entity.transform);
        }

        protected abstract void SwitchToMove();
        protected abstract void SwitchToDodge();

        private void HandleHolderChanged(Entity entity)
        {
            if (entity == null)
                return;

            if (_stateDataHolder.Teammates.Contains(entity) == false)
                SwitchToDodge();
        }
    
        private async UniTaskVoid RunIdleMovementLoop(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                float standTime = Random.Range(_stateDataHolder.EntityConfig.IdleMinStandTime, _stateDataHolder.EntityConfig.IdleMaxStandTime);
                Vector3 target = _stateDataHolder.AreaPointSelector.GetRandomPointInZone(_stateDataHolder.SquadZone, _stateDataHolder.Entity.transform.position);

                _stateDataHolder.AnimatorController.DodgeIdle();
                await _stateDataHolder.Mover.MoveTo(target, _stateDataHolder.EntityConfig.WalkSpeed, token);

                _stateDataHolder.AnimatorController.Idle();
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

                GameStatusService.Instance.CurrentBall.Chargeable.Charged += OnCharged;
            }
        }

        private void OnCharged()
        {
            GameStatusService.Instance.CurrentBall.Chargeable.Charged -= OnCharged;
        
            if (GameStatusService.Instance.IsBallFree)
            {
                if (GameStatusService.Instance.CurrentZone == _stateDataHolder.SquadZone)
                {
                    SwitchToMove();
                }
            }
        }
    }
}