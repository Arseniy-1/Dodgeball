using System.Threading;
using UnityEngine;

public abstract class EntityDodgeState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly CollisionHandler _collisionHandler;
    private readonly HitCheker _hitChecker;
    private readonly Collider _collider;

    private CancellationTokenSource _cancellationTokenSource;

    protected IStateSwitcher StateSwitcher;
    
    protected EntityDodgeState(
        AnimatorController animatorController,
        CollisionHandler collisionHandler,
        HitCheker hitChecker,
        Collider collider)
    {
        _animatorController = animatorController;
        _collisionHandler = collisionHandler;
        _hitChecker = hitChecker;
        _collider = collider;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        
        _hitChecker.enabled = true;
        _hitChecker.DetectBallHit += HandleBallDodge;
            
        _collisionHandler.enabled = false;
        _collider.isTrigger = true;
        AudioID.Jump.PlayOneShot();
        Jump(_cancellationTokenSource.Token);
    }

    public virtual void Exit()
    {
        if(_hitChecker != null)
            _hitChecker.enabled = false;
        
        _hitChecker.DetectBallHit -= HandleBallDodge;

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