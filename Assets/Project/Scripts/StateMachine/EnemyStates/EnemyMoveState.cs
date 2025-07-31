using System.Collections.Generic;
using Project.Scripts.Entities;
using Project.Scripts.StateMachine.EntityStates;
using UnityEngine;

namespace Project.Scripts.StateMachine.EnemyStates
{
    public class EnemyMoveState : EntityMoveState
    {
        private readonly Enemy _enemy;   
        private readonly List<Entity> _teammates;

        public EnemyMoveState(StateDataHolder dataHolder) 
            : base(dataHolder)
        {
            _enemy = (Enemy)dataHolder.Entity;
            _teammates = dataHolder.Teammates;
        }

        protected override void OnBallDetected(Ball ball)
        {
            StateDataHolder.BallHolder.EquipBall(ball, _enemy);
            StateSwitcher.SwitchState<EnemyAttackState>();
        }

        protected override void OnBallZoneChanged(Collider zone)
        {
            if (zone !=  StateDataHolder.SquadZone)
                StateSwitcher.SwitchState<EnemyDodgeReadyState>();
        }

        protected override void OnHolderChanged(Entity entity)
        {
            if (entity == null)
                return;
        
            if (_teammates.Contains(entity) == false)
                StateSwitcher.SwitchState<EnemyDodgeReadyState>();
            else if (entity != _enemy)
                StateSwitcher.SwitchState<EnemyIdleState>();
        }
    }
}