using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDodgeState : EntityDodgeState
{
    private readonly EnemyConfig _enemyConfig;
    
    private CancellationTokenSource _jumpCancellationTokenSource;

    public EnemyDodgeState(Enemy enemy, AnimatorController animatorController, Ball ball, Mover mover,
        Collider squadZone,
        Rigidbody rigidbody, EnemyConfig enemyConfig)
        : base(enemy, animatorController, ball, mover, squadZone, rigidbody, enemyConfig)
    {
        _enemyConfig = enemyConfig;
    }

    public override void Enter()
    {
        base.Enter();
        _jumpCancellationTokenSource = new CancellationTokenSource();
        RunJumpLoop(_jumpCancellationTokenSource.Token).Forget();
    }

    public override void Exit()
    {
        base.Exit();
        _jumpCancellationTokenSource?.Cancel();
        _jumpCancellationTokenSource?.Dispose();
        _jumpCancellationTokenSource = null;
    }

    private async UniTaskVoid RunJumpLoop(CancellationToken token)
    {
        while (token.IsCancellationRequested == false)
        {
            float waitTime = Random.Range(_enemyConfig.DodgeJumpDelayMinTime, _enemyConfig.DodgeJumpDelayMaxTime);
            await UniTask.Delay((int)(waitTime * 1000), cancellationToken: _jumpCancellationTokenSource.Token);
            StateSwitcher.SwitchState<EnemyJumpState>();
        }
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone == SquadZone)
            StateSwitcher.SwitchState<EnemyMoveState>();
    }
}