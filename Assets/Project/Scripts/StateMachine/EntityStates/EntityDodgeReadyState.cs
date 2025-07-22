using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityDodgeReadyState : IState
    {
        private readonly Entity _entity;
        private readonly AnimatorController _animatorController;
        private readonly Ball _ball;
        private readonly Mover _mover;
        private readonly Rigidbody _rigidbody;
        private readonly AreaPointSelector _areaPointSelector;
        private readonly Rotator _rotator;
        private readonly EntityConfig _entityConfig;
    
        protected readonly Collider SquadZone;

        private CancellationTokenSource _cancellationTokenSource;
    
        protected IStateSwitcher StateSwitcher;

        protected EntityDodgeReadyState(
            Entity entity,
            AnimatorController animatorController,
            Ball ball,
            Mover mover,
            Collider squadZone,
            Rigidbody rigidbody,
            EntityConfig entityConfig)
        {
            _entity = entity;
            _animatorController = animatorController;
            _ball = ball;
            _mover = mover;
            SquadZone = squadZone;
            _rigidbody = rigidbody;
            _entityConfig = entityConfig;
            _areaPointSelector = new AreaPointSelector();
            _rotator = new Rotator();
        }

        public void Initialize(IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        public virtual void Enter()
        {
            _animatorController.PrepareToBattle();
            
            _cancellationTokenSource = new CancellationTokenSource();
            _animatorController.DodgeIdle();
            _rigidbody.isKinematic = true;

            CheckBallStatus(_cancellationTokenSource.Token).Forget();
            RunDodgeMovementLoop(_cancellationTokenSource.Token).Forget();
        }

        public virtual void Exit()
        {
            _cancellationTokenSource.Cancel();
            _rigidbody.isKinematic = false;
        }

        public virtual void Update()
        {
            if (_ball != null)
                _rotator.RotateToTarget(_ball.transform, _entity.transform, _entityConfig.RotationSpeed);
        }

        protected abstract void HandleBallZoneChanged(Collider zone);

        private async UniTaskVoid CheckBallStatus(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                await UniTask.WaitForFixedUpdate();
                HandleBallZoneChanged(GameStatusService.Instance.CurrentZone);
            }
        }
    
        private async UniTaskVoid RunDodgeMovementLoop(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                float standTime = Random.Range(
                    _entityConfig.DodgeDirectionChangeMinTime,
                    _entityConfig.DodgeDirectionChangeMaxTime);

                Vector3 target = _areaPointSelector.GetRandomPointInZone(SquadZone, _entity.transform.position);
                _animatorController.DodgeIdle();

                if (token.IsCancellationRequested)
                    return;

                await _mover.MoveTo(target, _entityConfig.DodgeSpeed, token);

                if (token.IsCancellationRequested)
                    return;

                _animatorController.Idle();

                await UniTask.Delay(TimeSpan.FromSeconds(standTime), cancellationToken: token);

                if (token.IsCancellationRequested)
                    return;
            }
        }
    }
}