using System.Threading;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.Services.EffectServiceSystem;

namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityDodgeState : IState
    {
        private readonly StateDataHolder _stateDataHolder;

        private CancellationTokenSource _cancellationTokenSource;

        protected IStateSwitcher StateSwitcher;

        protected EntityDodgeState(StateDataHolder dataHolder)
        {
            _stateDataHolder = dataHolder;
        }

        public void Initialize(IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        public virtual void Enter()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _stateDataHolder.HitDetector.enabled = true;
            _stateDataHolder.HitDetector.BallHitDetected += HandleBallDodge;

            _stateDataHolder.CollisionHandler.enabled = false;
            _stateDataHolder.Collider.isTrigger = true;
            AudioID.Jump.PlayOneShot();
            Jump(_cancellationTokenSource.Token);
        }

        public virtual void Exit()
        {
            if (_stateDataHolder.HitDetector != null)
                _stateDataHolder.HitDetector.enabled = false;

            _stateDataHolder.HitDetector.BallHitDetected -= HandleBallDodge;

            if (_stateDataHolder.Collider != null)
                _stateDataHolder.CollisionHandler.enabled = true;

            if (_stateDataHolder.Collider != null)
                _stateDataHolder.Collider.isTrigger = false;

            _cancellationTokenSource.Cancel();
        }

        public virtual void Update()
        {
        }

        protected abstract void OnJumpFinished();

        private async void Jump(CancellationToken cancellationToken)
        {
            await _stateDataHolder.AnimatorController.Dodge();

            if (cancellationToken.IsCancellationRequested)
                return;

            OnJumpFinished();
        }

        private void HandleBallDodge()
        {
            AudioID.Dodge.PlayOneShot();
            EffectID.Joy.PlayEffect(_stateDataHolder.Collider.transform);
        }
    }
}