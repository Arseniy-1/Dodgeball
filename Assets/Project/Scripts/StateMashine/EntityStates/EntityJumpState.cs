using UnityEngine;

public abstract class EntityJumpState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly CollisionHandler _collisionHandler;
    private readonly HitCheker _hitCheker;
    private readonly Collider _collider;

    protected IStateSwitcher StateSwitcher;

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
        _hitCheker.enabled = true;
        _hitCheker.DetectBallHit += HandleBallDodge;
            
        _collisionHandler.enabled = false;
        _collider.isTrigger = true;
        MessageBrokerHolder.GameActions.Publish(new M_EntityJumped(_collider.transform));
        Jump();
    }

    public virtual void Exit()
    {
        _hitCheker.enabled = false;
        _hitCheker.DetectBallHit -= HandleBallDodge;
        
        _collisionHandler.enabled = true;
        _collider.isTrigger = false;
    }

    public virtual void Update() { }
    
    protected abstract void OnJumpFinished();

    private async void Jump()
    {
        await _animatorController.Dodge();
        OnJumpFinished();
    }

    private void HandleBallDodge()
    {
        MessageBrokerHolder.GameActions.Publish(new M_EntityDodged(_hitCheker.transform));
    }
}