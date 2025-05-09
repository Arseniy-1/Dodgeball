using UnityEngine;
using UniRx;

public class EnemyIdleState : IState
{
    private readonly Enemy _enemy;
    private IStateSwitcher _stateSwitcher;
    private Collider _squadZone;
    private CollisionHandler _collisionHandler;
    private BallHolder _ballHolder;
    private CompositeDisposable _disposable;

    public EnemyIdleState(Enemy enemy, CollisionHandler collisionHandler, Collider squadZone, BallHolder ballHolder,
        CompositeDisposable compositeDisposable)
    {
        _enemy = enemy;
        _squadZone = squadZone;
        _disposable = compositeDisposable;
        _collisionHandler = collisionHandler;
        _ballHolder = ballHolder;
        
        MessageBrokerHolder.GameActions.Receive<M_BallChangedZone>().Subscribe((message) => HandleBallZoneChanged(message.Zone))
            .AddTo(_disposable);
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        _collisionHandler.enabled = false;
    }

    public virtual void Exit()
    {
        _collisionHandler.enabled = true;
    }

    public virtual void Update()
    {
    }
    
    private void HandleBallZoneChanged(Collider collider)
    {
   
    }
}