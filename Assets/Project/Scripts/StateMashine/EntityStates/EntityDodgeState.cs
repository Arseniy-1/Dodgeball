using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class EntityDodgeState : IState
{
    private readonly Entity _entity;
    private readonly AnimatorController _animatorController;
    private readonly Ball _ball;
    private readonly Mover _mover;
    private readonly Rigidbody _rigidbody;
    private readonly AreaPointSelector _areaPointSelector;
    private readonly Rotator _rotator;
    private readonly EntityConfig _entityConfig;

    protected readonly Collider SquadZone;
    protected IStateSwitcher StateSwitcher;

    private CancellationTokenSource _cancellationTokenSource;
    private CompositeDisposable _disposable;

    protected EntityDodgeState(
        Entity entity, AnimatorController animatorController, Ball ball,
        Mover mover, Collider squadZone, Rigidbody rigidbody,
        EntityConfig entityConfig)
    {
        _entity = entity;
        _animatorController = animatorController;
        _ball = ball;
        _mover = mover;
        SquadZone = squadZone;
        _rigidbody = rigidbody;
        _entityConfig = entityConfig;
        _areaPointSelector = new AreaPointSelector();
        _rotator = new Rotator();
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        StateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _disposable = new CompositeDisposable();
        _animatorController.DodgeIdle();
        _rigidbody.isKinematic = true;
        
        MessageBrokerHolder.GameActions
            .Receive<M_BallChangedZone>()
            .Subscribe(message => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);
        
        RunDodgeMovementLoop(_cancellationTokenSource.Token).Forget();
    }

    public virtual void Exit()
    {
        _cancellationTokenSource?.Cancel();
        _disposable.Dispose();
        
        _rigidbody.isKinematic = false;
    }

    public virtual void Update()
    {
        if (_ball != null)
            _rotator.RotateToTarget(_ball.transform, _entity.transform, _entityConfig.RotationSpeed);
    }

    protected abstract void HandleBallZoneChanged(Collider zone);

    private async UniTaskVoid RunDodgeMovementLoop(CancellationToken token)
    {
        Debug.Log("Enter to dodge movement Wait Move " + _entity.gameObject.name);
        
        while (token.IsCancellationRequested == false)
        {
            float standTime = Random.Range(
                _entityConfig.DodgeDirectionChangeMinTime,
                _entityConfig.DodgeDirectionChangeMaxTime
            );

            Vector3 target = _areaPointSelector.GetRandomPointInZone(SquadZone, _entity.transform.position);
            _animatorController.DodgeIdle();
            Debug.Log("Dodge movement Wait Move " + _entity.gameObject.name);
            await _mover.MoveTo(target, _entityConfig.DodgeSpeed, token);
            _animatorController.Idle();
            Debug.Log("Dodge movement Wait StandTime " + _entity.gameObject.name + " Stand Time: " + standTime);
            await UniTask.Delay((int)(standTime * 1000), cancellationToken: token);
        }
    }
}