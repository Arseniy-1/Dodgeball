using UnityEngine;

public class PlayerDodgeState : IState
{
    private readonly Player _player;
    private IStateSwitcher _stateSwitcher;

    private Vector3 _targetPosition;
    private bool _canMove;

    // private CancellationTokenSource _source;

    public PlayerDodgeState(Player player)
    {
        _player = player;
    }

    public void Initialize(IStateSwitcher stateSwitcher)
    {
        // _source = new CancellationTokenSource();
        // _stateSwitcher = stateSwitcher;
    }

    public void Enter()
    {
        // WaitingAfterMove().Forget();
        // _player.PlayerInputController.ActionButtonStarted += Jump;
        //
        // if (_player.CollisionHandler != null)
        //     _player.CollisionHandler.enabled = false;
    }

    public void Exit()
    {
        // _player.PlayerInputController.ActionButtonStarted -= Jump;
        //
        // if (_player.CollisionHandler != null)
        //     _player.CollisionHandler.enabled = true;
        //     
        // _source.Cancel();
    }

    public void Update()
    {
        // if (_canMove)
        // {
        //     _player.Mover.Move(_targetPosition, _player.PlayerStats.DodgeSpeed, _player.PlayerStats.RotationSpeed);
        // }
        //
        // if (Vector3.Distance(_player.transform.position, _targetPosition) <= 0.5f && _canMove)
        // {
        //     WaitingAfterMove().Forget();
        // }
        //
        // if (_player.TargetProvider.Ball != null)
        // {
        //     _stateSwitcher.SwitchState<PlayerIdleState>();
        // }
    }

    // private async UniTaskVoid WaitingAfterMove()
    // {
    //     if (_source.IsCancellationRequested)
    //         return;
    //
    //     _canMove = false;
    //
    //     float waitTime = Random.Range(_player.PlayerStats.DodgeDirectionChangeMinTime,
    //         _player.PlayerStats.DodgeDirectionChangeMaxTime);
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
    //
    // private void Jump()
    // {
    //     _stateSwitcher.SwitchState<PlayerJumpState>();
    // }
}