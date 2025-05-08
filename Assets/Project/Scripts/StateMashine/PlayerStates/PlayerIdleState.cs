using UnityEngine;

public class PlayerIdleState : IState
{
    private readonly Player _player;
    private IStateSwitcher _stateSwitcher;


    private Vector3 _targetPosition;
    private bool _canMove;

    // private CancellationTokenSource _source;

    public PlayerIdleState(Player player)
    {
        _player = player;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        // _source = new CancellationTokenSource();
        // _stateSwitcher = stateSwitcher;
    }

    public virtual void Enter()
    {
        // WaitingAfterMove().Forget();
        // _player.Rigidbody.isKinematic = true;
        // _player.Collider.isTrigger = true;
        // _player.CollisionHandler.enabled = false;
    }

    public virtual void Exit()
    {
        // _canMove = false;
        // _player.Rigidbody.isKinematic = false;
        // _player.Collider.isTrigger = false;
        // _player.CollisionHandler.enabled = true;
    }

    public virtual void Update()
    {
        // if (_canMove)
        // {
        //     _player.Mover.Move(_targetPosition, _player.PlayerStats.Speed, _player.PlayerStats.RotationSpeed);
        // }
        //
        // if (Vector3.Distance(_player.transform.position, _targetPosition) <= 0.5f && _canMove)
        // {
        //     WaitingAfterMove().Forget();
        // }
        //     
        // if (_player.TargetProvider.Ball != null)
        //     _stateSwitcher.SwitchState<PlayerMoveState>();
    }

    // private async UniTaskVoid WaitingAfterMove()
    // {
    //     if (_source.IsCancellationRequested)
    //         return;
    //
    //     _canMove = false;
    //
    //     float waitTime = Random.Range(_player.PlayerStats.IdleMinStandTime, _player.PlayerStats.IdleMinStandTime);
    //     await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: _source.Token);
    //
    //     if (_source.Token.IsCancellationRequested)
    //         return;
    //
    //     _canMove = true;
    //
    //     GetTargetPosition();
    // }
    //
    // private void GetTargetPosition()
    // {
    //     Bounds bounds = _player.PlayZone.bounds;
    //     float randomX = Random.Range(bounds.min.x, bounds.max.x);
    //     float randomZ = Random.Range(bounds.min.z, bounds.max.z);
    //     _targetPosition = new Vector3(randomX, _player.transform.position.y, randomZ);
    // }
}