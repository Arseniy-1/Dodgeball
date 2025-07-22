using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using Project.Scripts.StateMachine.EntityStates;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyPrepareState : EntityPrepareState
    {
        public EnemyPrepareState(
            Enemy enemy,
            AnimatorController animatorController,
            TargetScanner targetScanner,
            List<Entity> teammates)
            : base(
                enemy,
                animatorController,
                targetScanner,
                teammates)
        {
        }

        protected override void HandleStartGame()
        {
            StateSwitcher.SwitchState<EnemyIdleState>();
        }
    }
}