using UnityEngine;

public abstract class EntityJumpState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;

    protected IStateSwitcher StateSwitcher;

    protected EntityJumpState(
        AnimatorController animatorController,
        GroundChecker groundChecker,
        CollisionHandler collisionHandler,
        Collider collider)
    {
        _animatorController = animatorController;
        _collisionHandler = collisionHandler;
        _collider = collider;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _collisionHandler.enabled = false;
        _collider.isTrigger = true;
        Jump();
    }

    public virtual void Exit()
    {
        _collisionHandler.enabled = true;
        _collider.isTrigger = false;
    }

    public virtual void Update() { }

    private async void Jump()
    {
        await _animatorController.Dodge();
        OnJumpFinished();
    }

    protected abstract void OnJumpFinished();
}