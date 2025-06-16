using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDodgeState : EntityDodgeState
{
    private readonly EnemyStats _enemyStats;
    private IDisposable _jumpLoopDisposable;

    public EnemyDodgeState(Enemy enemy, AnimatorController animatorController, Ball ball, Mover mover, Collider squadZone, 
        Rigidbody rigidbody, EnemyStats enemyStats)
        : base(enemy, animatorController, ball, mover, squadZone, rigidbody, enemyStats)
    {
        _enemyStats = enemyStats;
    }

    public override void Enter()
    {
        base.Enter();
        StartJumpLoop();
    }

    public override void Exit()
    {
        base.Exit();
        _jumpLoopDisposable?.Dispose();
    }

    private void StartJumpLoop()
    {
        _jumpLoopDisposable = Observable.FromCoroutine(JumpLoop)
            .Subscribe()
            .AddTo(Disposable);
    }

    private System.Collections.IEnumerator JumpLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(_enemyStats.DodgeJumpDelayMinTime, _enemyStats.DodgeJumpDelayMaxTime);
            yield return new WaitForSeconds(waitTime);
            StateSwitcher.SwitchState<EnemyJumpState>();
        }
    }

    protected override void HandleBallZoneChanged(Collider zone)
    {
        if (zone == SquadZone)
            StateSwitcher.SwitchState<EnemyMoveState>();
    }
}