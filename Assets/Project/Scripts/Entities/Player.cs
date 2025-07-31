using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.Services.EffectServiceSystem;
using Project.Scripts.StateMachine;
using Project.Scripts.StateMachine.PlayerStates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Entities
{
    public class Player : Entity, IDestoyable<Player>
    {
        [SerializeField] private PlayerInputController _playerInputController;
        [SerializeField] private PlayerConfig _playerConfig;

        public event Action<Player> Destroyed;

        public override void Celebrate()
        {
            StateMachine.SwitchState<PlayerCelebrateState>();
            BallHolder.LostBall();
        }

        [Button]
        public override void Die()
        {
            base.Die();
            Destroyed?.Invoke(this);
        }

        [Button]
        protected override async UniTaskVoid HandleLostHealth(CancellationToken token)
        {
            StateMachine.SwitchState<PlayerDeathState>();
            HealthCanvas.gameObject.SetActive(false);
            EffectID.Death.PlayEffect(transform);
            AudioID.Dead.PlayOneShot();

            await AnimatorController.Death();
            await HideEntity(token);

            Die();
        }

        protected override List<IState> CreateStates()
        {
            var dataHolder = new StateDataHolder(this, CollisionHandler, Collider, SquadZone,
                Rigidbody, AnimatorController, BallHolder, TargetScanner,
                TargetProvider, Teammates, BallThrower, Mover, _playerConfig, HitDetector);

            return new List<IState>
            {
                new PlayerPrepareState(dataHolder),
                new PlayerCelebrateState(dataHolder, _playerInputController),
                new PlayerIdleState(dataHolder),
                new PlayerMoveState(dataHolder),
                new PlayerDodgeReadyState(dataHolder, _playerInputController),
                new PlayerAttackState(dataHolder, _playerInputController),
                new PlayerDodgeState(dataHolder),
                new PlayerDeathState(dataHolder),
            };
        }

        protected override EntityConfig GetConfig()
        {
            return _playerConfig;
        }
    }
}