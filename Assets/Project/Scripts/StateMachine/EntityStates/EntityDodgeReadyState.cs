using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityDodgeReadyState : IState
    {
        private readonly StateDataHolder _stateDataHolder;

        private CancellationTokenSource _cancellationTokenSource;
    
        protected IStateSwitcher StateSwitcher;

        protected EntityDodgeReadyState(StateDataHolder dataHolder)
        {
            _stateDataHolder = dataHolder;;
        }

        public void Initialize(IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        public virtual void Enter()
        {
            _stateDataHolder.AnimatorController.PrepareToBattle();
            
            _cancellationTokenSource = new CancellationTokenSource();
            _stateDataHolder.AnimatorController.DodgeIdle();
            _stateDataHolder.Rigidbody.isKinematic = true;

            CheckBallStatus(_cancellationTokenSource.Token).Forget();
            RunDodgeMovementLoop(_cancellationTokenSource.Token).Forget();
        }

        public virtual void Exit()
        {
            _cancellationTokenSource.Cancel();
            _stateDataHolder.Rigidbody.isKinematic = false;
        }

        public virtual void Update()
        {
            var ball = GameStatusService.Instance.CurrentBall;
            
            if (ball != null)
                _stateDataHolder.Rotator.RotateToTarget(
                    ball.transform,
                    _stateDataHolder.Entity.transform,
                    _stateDataHolder.EntityConfig.RotationSpeed);
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
                    _stateDataHolder.EntityConfig.DodgeDirectionChangeMinTime,
                    _stateDataHolder.EntityConfig.DodgeDirectionChangeMaxTime);

                Vector3 target = _stateDataHolder.AreaPointSelector.GetRandomPointInZone(_stateDataHolder.SquadZone, _stateDataHolder.Entity.transform.position);
                _stateDataHolder.AnimatorController.DodgeIdle();

                if (token.IsCancellationRequested)
                    return;

                await _stateDataHolder.Mover.MoveTo(target, _stateDataHolder.EntityConfig.DodgeSpeed, token);

                if (token.IsCancellationRequested)
                    return;

                _stateDataHolder.AnimatorController.Idle();

                await UniTask.Delay(TimeSpan.FromSeconds(standTime), cancellationToken: token);

                if (token.IsCancellationRequested)
                    return;
            }
        }
    }
}