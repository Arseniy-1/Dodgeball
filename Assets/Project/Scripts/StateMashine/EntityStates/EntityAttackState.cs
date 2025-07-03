using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class EntityAttackState  : IState
{
    private readonly Entity _entity;
    private readonly AnimatorController _animatorController;
    private readonly BallHolder _ballHolder;
    private readonly TargetScanner _targetScanner;
    private readonly List<Entity> _teammates;
    private readonly BallThrower _ballThrower;
    private readonly Rotator _rotator;  

    protected readonly TargetProvider TargetProvider;
    protected readonly CollisionHandler CollisionHandler;
    protected readonly Collider Collider;
    protected readonly Rigidbody Rigidbody;

    protected IStateSwitcher StateSwitcher; 

    public EntityAttackState(Entity entity, CollisionHandler collisionHandler,
        Collider collider, Rigidbody rigidbody, AnimatorController animatorController, BallHolder ballHolder,
        TargetScanner targetScanner, TargetProvider targetProvider,
        List<Entity> teammates, BallThrower ballThrower)
    {
        _entity = entity;
        CollisionHandler = collisionHandler;
        Collider = collider;
        Rigidbody = rigidbody;
        _animatorController = animatorController;
        _ballHolder = ballHolder;
        _targetScanner = targetScanner;
        TargetProvider = targetProvider;
        _teammates = teammates;
        _ballThrower = ballThrower;
        
        _rotator = new Rotator();
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        Entity target = _targetScanner.Scan(_teammates);
        TargetProvider.SelectTarget(target);
        
        Rigidbody.isKinematic = true;
        CollisionHandler.enabled = false;
        Collider.enabled = false;

        _animatorController.Idle();
    }

    public virtual void Exit()
    {
        Debug.Log("Exit Attack " + _entity.gameObject.name);
        Rigidbody.isKinematic = false;
        CollisionHandler.enabled = true;
        Collider.enabled = true;
    }

    public virtual void Update()
    {
        if (TargetProvider.Target != null)
        {
            _rotator.RotateToTarget(TargetProvider.Target.transform, _entity.transform);
        }
    }
    
    protected void StartAttack()
    {
        _animatorController.PrepareAttack();
        _ballThrower.StartCharging();
    }

    protected UniTask ThrowBall()
    {
        Ball ball = _ballHolder.LostBall();
        _ballThrower.StopCharging();
        _ballThrower.Throw(ball);

        return _animatorController.Attack();
    }
}