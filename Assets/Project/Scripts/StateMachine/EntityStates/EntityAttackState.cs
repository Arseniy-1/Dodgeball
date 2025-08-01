using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Entities;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.Services.Ball;

namespace Project.Scripts.StateMachine.EntityStates
{
    public class EntityAttackState : IState
    {
        private readonly StateDataHolder _stateDataHolder;
        
        private CancellationTokenSource _cancellationTokenSource;

        protected IStateSwitcher StateSwitcher;

        protected EntityAttackState(StateDataHolder dataHolder)
        {
            _stateDataHolder = dataHolder;
        }

        public virtual void Enter()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _stateDataHolder.Rigidbody.isKinematic = true;
            _stateDataHolder.CollisionHandler.enabled = false;
            _stateDataHolder.Collider.enabled = false;

            _stateDataHolder.AnimatorController.Idle();
        }

        public virtual void Exit()
        {
            _cancellationTokenSource.Cancel();

            _stateDataHolder.Rigidbody.isKinematic = false;
            _stateDataHolder.CollisionHandler.enabled = true;
            _stateDataHolder.Collider.enabled = true;
        }

        public void Initialize(IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        public virtual void Update()
        {
            if (_stateDataHolder.TargetProvider.Target != null)
            {
                _stateDataHolder.Rotator.RotateToTarget(
                    _stateDataHolder.TargetProvider.Target.transform,
                    _stateDataHolder.Entity.transform);
            }
        }

        protected void StartAttack()
        {
            _stateDataHolder.AnimatorController.PrepareAttack();
            _stateDataHolder.BallThrower.StartCharging();
        }

        protected UniTask ThrowBall()
        {
            Ball ball = _stateDataHolder.BallHolder.LostBall();
            _stateDataHolder.BallThrower.StopCharging();
            _stateDataHolder.BallThrower.Throw(ball);

            AudioID.Attack.PlayOneShot();

            return _stateDataHolder.AnimatorController.Attack();
        }

        protected async UniTask ApplyTarget()
        {
            Entity target = await FindTarget(_cancellationTokenSource.Token);
            
            if(_cancellationTokenSource.Token.IsCancellationRequested)
                return;
            
            _stateDataHolder.TargetProvider.SelectTarget(target);
        }

        private async UniTask<Entity> FindTarget(CancellationToken token)
        {
            Entity target = _stateDataHolder.TargetScanner.Scan(_stateDataHolder.Teammates);

            while (token.IsCancellationRequested == false && target == null)
            {
                target = _stateDataHolder.TargetScanner.Scan(_stateDataHolder.Teammates);

                await UniTask.NextFrame(cancellationToken: token);
            }

            return target;
        }
    }
}