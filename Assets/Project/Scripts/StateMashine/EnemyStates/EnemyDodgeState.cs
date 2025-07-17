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
        Debug.Log("*");
    }

    public override void Exit()
    {
        base.Exit();
        _jumpCancellationTokenSource.Cancel();
        Debug.Log(_jumpCancellationTokenSource.Token.IsCancellationRequested);
        Debug.Log("#");
    }

    private async UniTaskVoid RunJumpLoop(CancellationToken token)
    {
        Debug.Log(token.IsCancellationRequested);
        while (token.IsCancellationRequested == false)
        {
            float waitTime = Random.Range(_enemyConfig.DodgeJumpDelayMinTime, _enemyConfig.DodgeJumpDelayMaxTime);
            await UniTask.Delay((int)(waitTime * 1000), cancellationToken: token);

            if (token.IsCancellationRequested)
                return;

            StateSwitcher.SwitchState<EnemyJumpState>();
        }
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (GameStatusService.Instance.CurrentBall.Chargeable.IsCharged)
            return;

        if (GameStatusService.Instance.CurrentHolder != null)
            return;

        if (zone == SquadZone)
        {
            StateSwitcher.SwitchState<EnemyMoveState>();
        }
    }
}