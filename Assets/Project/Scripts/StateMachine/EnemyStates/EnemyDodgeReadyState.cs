using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyDodgeReadyState : EntityDodgeReadyState
    {
        private readonly EnemyConfig _enemyConfig;

        private CancellationTokenSource _jumpCancellationTokenSource;

        public EnemyDodgeReadyState(
            Enemy enemy,
            AnimatorController animatorController,
            Ball ball,
            Mover mover,
            Collider squadZone,
            Rigidbody rigidbody,
            EnemyConfig enemyConfig)
            : base(enemy,
                animatorController,
                ball,
                mover,
                squadZone,
                rigidbody,
                enemyConfig)
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
        }

        protected override void HandleBallZoneChanged(Collider zone)
        {
            if (GameStatusService.Instance.IsBallFree == false)
                return;

            if (zone == SquadZone)
                StateSwitcher.SwitchState<EnemyMoveState>();
        }

        private async UniTaskVoid RunJumpLoop(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                float waitTime = Random.Range(_enemyConfig.DodgeJumpDelayMinTime, _enemyConfig.DodgeJumpDelayMaxTime);

                await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: token);

                if (token.IsCancellationRequested)
                    return;

                StateSwitcher.SwitchState<EnemyDodgeState>();
            }
        }
    }
}