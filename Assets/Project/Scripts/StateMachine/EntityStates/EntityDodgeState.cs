using System.Threading;
using Project.Scripts.Services;
using Project.Scripts.Services.AudioService;
using Project.Scripts.Services.EffectService;
using UnityEngine;

namespace Project.Scripts.StateMachine.EntityStates
{
    public abstract class EntityDodgeState : IState
    {
        private readonly AnimatorController _animatorController;
        private readonly CollisionHandler _collisionHandler;
        private readonly HitDetector _hitDetector;
        private readonly Collider _collider;

        private CancellationTokenSource _cancellationTokenSource;

        protected IStateSwitcher StateSwitcher;
    
        protected EntityDodgeState(
            AnimatorController animatorController,
            CollisionHandler collisionHandler,
            HitDetector hitDetector,
            Collider collider)
        {
            _animatorController = animatorController;
            _collisionHandler = collisionHandler;
            _hitDetector = hitDetector;
            _collider = collider;
        }

        public void Initialize(IStateSwitcher stateSwitcher)
        {
            StateSwitcher = stateSwitcher;
        }

        public virtual void Enter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        
            _hitDetector.enabled = true;
            _hitDetector.DetectBallHit += HandleBallDodge;
            
            _collisionHandler.enabled = false;
            _collider.isTrigger = true;
            AudioID.Jump.PlayOneShot();
            Jump(_cancellationTokenSource.Token);
        }

        public virtual void Exit()
        {
            if(_hitDetector != null)
                _hitDetector.enabled = false;
        
            _hitDetector.DetectBallHit -= HandleBallDodge;

            if(_collider != null)
                _collisionHandler.enabled = true;
        
            if(_collider != null)
                _collider.isTrigger = false;
        
            _cancellationTokenSource.Cancel();
        }

        public virtual void Update() { }
    
        protected abstract void OnJumpFinished();

        private async void Jump(CancellationToken cancellationToken)
        {
            await _animatorController.Dodge();
        
            if(cancellationToken.IsCancellationRequested)
                return;
        
            OnJumpFinished();
        }

        private void HandleBallDodge()
        {
            AudioID.Dodge.PlayOneShot();
            EffectID.Joy.PlayEffect(_collider.transform);
        }
    }
}