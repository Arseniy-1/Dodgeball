using UnityEngine;

public abstract class EntityDeathState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;
    private readonly BallHolder _ballHolder;
    private readonly BallThrower _ballThrower;

    protected EntityDeathState(AnimatorController animatorController, CollisionHandler collisionHandler, 
        Collider collider, BallHolder ballHolder, BallThrower ballThrower)
    {
        _animatorController = animatorController;
        _collisionHandler = collisionHandler;
        _collider = collider;
        _ballHolder = ballHolder;
        _ballThrower = ballThrower;
    }

    public virtual void Enter()
    {
        _animatorController.Death();
        
        _ballThrower.StopCharging();
        _ballHolder.LostBall();
        _collisionHandler.enabled = false;
        _collider.enabled = false;
    }

    public virtual void Exit()
    {
        _collisionHandler.enabled = true;
        _collider.enabled = true;
    }
    
    public void Initialize(IStateSwitcher stateSwitcher)
    {
    }

    public virtual void Update() { }
}