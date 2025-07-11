using System.Threading;
using UnityEngine;

public abstract class EntityJumpState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly CollisionHandler _collisionHandler;
    private readonly HitCheker _hitCheker;
    private readonly Collider _collider;

    protected IStateSwitcher StateSwitcher;

    private CancellationTokenSource _cancellationTokenSource;
    
    protected EntityJumpState(
        AnimatorController animatorController,
        CollisionHandler collisionHandler,
        HitCheker hitCheker,
        Collider collider)
    {
        _animatorController = animatorController;
        _collisionHandler = collisionHandler;
        _hitCheker = hitCheker;
        _collider = collider;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        
        _hitCheker.enabled = true;
        _hitCheker.DetectBallHit += HandleBallDodge;
            
        _collisionHandler.enabled = false;
        _collider.isTrigger = true;
        AudioID.Jump.PlayOneShot();
        Jump(_cancellationTokenSource.Token);
    }

    public virtual void Exit()
    {
        if(_hitCheker != null)
            _hitCheker.enabled = false;
        
        _hitCheker.DetectBallHit -= HandleBallDodge;

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