using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public class EntityAttackState : IState
{
    private readonly Entity _entity;
    private readonly AnimatorController _animatorController;
    private readonly BallHolder _ballHolder;
    private readonly TargetScanner _targetScanner;
    private readonly List<Entity> _teammates;
    private readonly BallThrower _ballThrower;
    private readonly Rotator _rotator;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;
    private readonly Rigidbody _rigidbody;
    
    private readonly TargetProvider _targetProvider;

    private CancellationTokenSource _cancellationTokenSource;
    
    protected IStateSwitcher StateSwitcher;

    protected EntityAttackState(Entity entity, CollisionHandler collisionHandler,
        Collider collider, Rigidbody rigidbody, AnimatorController animatorController, BallHolder ballHolder,
        TargetScanner targetScanner, TargetProvider targetProvider,
        List<Entity> teammates, BallThrower ballThrower)
    {
        _entity = entity;
        _collisionHandler = collisionHandler;
        _collider = collider;
        _rigidbody = rigidbody;
        _animatorController = animatorController;
        _ballHolder = ballHolder;
        _targetScanner = targetScanner;
        _targetProvider = targetProvider;
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
        _cancellationTokenSource = new CancellationTokenSource();
        
        _rigidbody.isKinematic = true;
        _collisionHandler.enabled = false;
        _collider.enabled = false;

        _animatorController.Idle();
    }

    public virtual void Exit()
    {
        _cancellationTokenSource.Cancel();
        
        _rigidbody.isKinematic = false;
        _collisionHandler.enabled = true;
        _collider.enabled = true;
    }

    public virtual void Update()
    {
        if (_targetProvider.Target != null)
        {
            _rotator.RotateToTarget(_targetProvider.Target.transform, _entity.transform);
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
        
        AudioID.Attack.PlayOneShot();
        
        return _animatorController.Attack();
    }

    protected async UniTask ApplyTarget()
    {
        Entity  target = await FindTarget(_cancellationTokenSource.Token);
        _targetProvider.SelectTarget(target);
    }
    
    private async UniTask<Entity> FindTarget(CancellationToken token)
    {
        Entity target = _targetScanner.Scan(_teammates);

        while (token.IsCancellationRequested == false && target == null)
        {
            target = _targetScanner.Scan(_teammates);

            await UniTask.NextFrame(cancellationToken: token);
        }

        return target;
    }
}