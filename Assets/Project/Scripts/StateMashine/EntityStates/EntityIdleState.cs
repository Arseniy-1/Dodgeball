using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class EntityIdleState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly Ball _ball;
    private readonly Mover _mover;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _collider;
    private readonly Rigidbody _rigidbody;
    private readonly AreaPointSelector _areaPointSelector;
    private readonly Rotator _rotator;
    private readonly Entity _entity;
    private readonly EntityConfig _entityConfig;

    private CompositeDisposable _disposable;
    private CancellationTokenSource _cancellationTokenSource;

    protected readonly Collider SquadZone;
    protected IStateSwitcher StateSwitcher;
    
    protected EntityIdleState(
        AnimatorController animatorController, Ball ball, Mover mover,
        CollisionHandler collisionHandler, Collider squadZone, 
        Collider collider, Rigidbody rigidbody, Entity entity, EntityConfig entityConfig)
    {
        _animatorController = animatorController;
        _ball = ball;
        _mover = mover;
        _collisionHandler = collisionHandler;
        SquadZone = squadZone;
        _collider = collider;
        _rigidbody = rigidbody;
        _entity = entity;
        _entityConfig = entityConfig;
        _areaPointSelector = new AreaPointSelector();
        _rotator = new Rotator();
    }

    public virtual void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _cancellationTokenSource = new CancellationTokenSource();
     
        _disposable = new CompositeDisposable();

        MessageBrokerHolder.GameActions
            .Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);
        
        _rigidbody.isKinematic = true;
        _collisionHandler.enabled = false;
        _collider.enabled = false;

        _animatorController.Idle();
        RunIdleMovementLoop(_cancellationTokenSource.Token).Forget();
    }

    public virtual void Exit()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
        
        _disposable.Dispose();
        
        _rigidbody.isKinematic = false;
        _collisionHandler.enabled = true;
        _collider.enabled = true;
    }
    
    private async UniTaskVoid RunIdleMovementLoop(CancellationToken token)
    {
        while (token.IsCancellationRequested == false)
        {
            float standTime = Random.Range(_entityConfig.IdleMinStandTime, _entityConfig.IdleMaxStandTime);
            Vector3 target = _areaPointSelector.GetRandomPointInZone(SquadZone, _entity.transform.position);
            
            _animatorController.DodgeIdle();
            
            if (token.IsCancellationRequested)
                return;
            
            Debug.Log("Idle movement loop " + _entity.gameObject.name);
            await _mover.MoveTo(target, _entityConfig.WalkSpeed, token);
                        
            if (token.IsCancellationRequested)
                return;
            
            _animatorController.Idle();
            await UniTask.Delay((int)(standTime * 1000), cancellationToken: token);
        }
    }

    public virtual void Update()
    {
        if (_ball != null)
            _rotator.RotateToTarget(_ball.transform, _entity.transform);
    }

    protected abstract void HandleBallZoneChanged(Collider zone);
}