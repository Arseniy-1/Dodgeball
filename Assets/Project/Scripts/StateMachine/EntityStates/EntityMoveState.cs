using System.Threading;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using Project.Scripts.Services.Ball;
using UnityEngine;

namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityMoveState : IState
    {
        private CancellationTokenSource _cancellationTokenSource;

        protected readonly StateDataHolder StateDataHolder;
        protected IStateSwitcher StateSwitcher;

        protected EntityMoveState(StateDataHolder dataHolder)
        {
            StateDataHolder = dataHolder;
        }

        public void Initialize(IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        public virtual void Enter()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            GameStatusService.Instance.HolderChanged += OnHolderChanged;
            GameStatusService.Instance.ZoneChanged += OnBallZoneChanged;
            StateDataHolder.CollisionHandler.BallDetected += OnBallDetected;

            StateDataHolder.CollisionHandler.enabled = true;
            StateDataHolder.Collider.enabled = true;
            StateDataHolder.Collider.isTrigger = false;

            StateDataHolder.AnimatorController.Run();
        }

        public virtual void Exit()
        {
            _cancellationTokenSource.Cancel();

            GameStatusService.Instance.HolderChanged -= OnHolderChanged;
            GameStatusService.Instance.ZoneChanged -= OnBallZoneChanged;
            StateDataHolder.CollisionHandler.BallDetected -= OnBallDetected;
        }

        public virtual void Update()
        {
            StateDataHolder.Rotator.RotateToTarget(
                GameStatusService.Instance.CurrentBall.transform, 
                StateDataHolder.Entity.transform,
                StateDataHolder.EntityConfig.RotationSpeed);
            
            StateDataHolder.Mover.FollowTarget(
                GameStatusService.Instance.CurrentBall.transform,
                StateDataHolder.EntityConfig.RunSpeed);
        }

        protected abstract void OnBallDetected(Ball ball);

        protected abstract void OnBallZoneChanged(Collider zone);

        protected abstract void OnHolderChanged(Entity entity);
    }
}