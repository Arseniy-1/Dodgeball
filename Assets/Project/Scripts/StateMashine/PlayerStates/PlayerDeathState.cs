using UniRx;
using UnityEngine;

public class PlayerDeathState : IState
{
    private readonly Player _player;
    private readonly PlayerStats _playerStats;
    private readonly CollisionHandler _collisionHandler;
    private readonly Collider _squadZone;
    private readonly CompositeDisposable _disposable;
    private readonly BallHolder _ballHolder;
    private readonly Ball _ball;
    private readonly Collider _collider;

    private IStateSwitcher _stateSwitcher;
    
    private Coroutine _moveRoutine;

    public PlayerDeathState(Player player, PlayerStats stats, Mover mover, CollisionHandler collisionHandler,
        Collider squadZone, CompositeDisposable compositeDisposable, BallHolder ballHolder, Ball ball, Collider collider)
    {
        _player = player;
        _playerStats = stats;
        _collisionHandler = collisionHandler;
        _squadZone = squadZone;
        _disposable = compositeDisposable;
        _ballHolder = ballHolder;
        _ball = ball;
        _collider = collider;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        _collisionHandler.enabled = false;
        _collider.enabled = false;
    }

    public void Exit()
    { 
        _collisionHandler.enabled = true;
        _collider.enabled = true;
    }

    public void Update()
    {
    }
}