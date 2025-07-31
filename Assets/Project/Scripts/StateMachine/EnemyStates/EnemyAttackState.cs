using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.StateMachine.EntityStates;
using Random = UnityEngine.Random;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyAttackState : EntityAttackState
    {
        private readonly EnemyConfig _enemyConfig;

        private CancellationTokenSource _attackCancelationTokenSource;

        public EnemyAttackState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
            _enemyConfig = (EnemyConfig)dataHolder.EntityConfig;
        }

        public override async void Enter()
        {
            _attackCancelationTokenSource = new CancellationTokenSource();

            base.Enter();
            await ApplyTarget();

            Attack(_attackCancelationTokenSource.Token);
        }

        public override void Exit()
        {
            _attackCancelationTokenSource.Cancel();
            base.Exit();
        }

        private async void Attack(CancellationToken token)
        {
            StartAttack();

            float shootDelay = Random.Range(_enemyConfig.MinThrowWait, _enemyConfig.MaxThrowWait);

            await UniTask.Delay(TimeSpan.FromSeconds(shootDelay), cancellationToken: token);
            await ThrowBall();
        
            StateSwitcher.SwitchState<EnemyIdleState>();
        }
    }
}