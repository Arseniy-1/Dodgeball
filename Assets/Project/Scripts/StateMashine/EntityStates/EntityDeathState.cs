using UnityEngine;

public abstract class EntityDeathState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;
    private readonly BallHolder _ballHolder;

    protected EntityDeathState(AnimatorController animatorController, CollisionHandler collisionHandler, Collider collider, BallHolder ballHolder)
    {
        _animatorController = animatorController;
        _collisionHandler = collisionHandler;
        _collider = collider;
        _ballHolder = ballHolder;
    }

    public virtual void Enter()
    {
        Debug.Log("EnterDeath");
        _animatorController.Death();
        _ballHolder.LostBall();
        _collisionHandler.enabled = false;
        _collider.enabled = false;
    }

    public virtual void Exit()
    {
        Debug.Log("ExitDeath");
        _collisionHandler.enabled = true;
        _collider.enabled = true;
    }
    
    public void Initialize(IStateSwitcher stateSwitcher)
    {
    }

    public virtual void Update() { }
}