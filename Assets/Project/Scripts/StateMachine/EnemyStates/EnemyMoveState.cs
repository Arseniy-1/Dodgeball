using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.Services;
using Project.Scripts.Services.Ball;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyMoveState : EntityMoveState
    {
        private readonly Enemy _enemy;   
        private readonly List<Entity> _teammates;

        public EnemyMoveState(
            Enemy enemy,
            AnimatorController animatorController,
            List<Entity> teammates,
            EnemyConfig enemyConfig,
            CollisionHandler collisionHandler,
            Collider squadZone,
            BallHolder ballHolder,
            Collider collider,
            Mover mover)
            : base(
                enemy,
                animatorController,
                collisionHandler,
                squadZone,
                ballHolder,
                collider,
                enemyConfig,
                mover)
        {
            _enemy = enemy;
            _teammates = teammates;
        }

        protected override void OnBallDetected(Ball ball)
        {
            BallHolder.EquipBall(ball, _enemy);
            StateSwitcher.SwitchState<EnemyAttackState>();
        }

        protected override void HandleBallZoneChanged(Collider zone)
        {
            if (zone != SquadZone)
                StateSwitcher.SwitchState<EnemyDodgeReadyState>();
        }

        protected override void HandleBallHolderChanged(Entity entity)
        {
            if (entity == null)
                return;
        
            if (_teammates.Contains(entity) == false)
            {
                StateSwitcher.SwitchState<EnemyDodgeReadyState>();
            }
            else if (entity != _enemy)
            {
                StateSwitcher.SwitchState<EnemyIdleState>();
            }
        }
    }
}