using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.Services.EffectService;
using Project.Scripts.StateMachine;
using Project.Scripts.StateMachine.PlayerStates;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.Entities
{
    public class Player : Entity, IDestoyable<Player>
    {
        [SerializeField] private PlayerInputController _inputController;
        [SerializeField] private PlayerConfig _playerConfig;

        public event Action<Player> OnDestroyed;
    
        public override void Celebrate()
        {
            StateMachine.SwitchState<PlayerCelebrateState>();
            BallHolder.LostBall();
        }

        [Button]
        public override void Die()
        {
            base.Die();
            OnDestroyed?.Invoke(this);
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
            return new List<IState>
            {
                new PlayerPrepareState(this, AnimatorController, TargetScanner, Teammates),
                new PlayerCelebrateState(this, AnimatorController, BallHolder, BallThrower, CollisionHandler, _inputController, Teammates),
                new PlayerIdleState(this,AnimatorController, Ball, Mover, CollisionHandler, SquadZone, Collider, Rigidbody, _playerConfig, Teammates),
                new PlayerMoveState(this, AnimatorController, Teammates, _playerConfig, CollisionHandler, SquadZone, BallHolder, Collider, Mover),
                new PlayerDodgeReadyState(this, AnimatorController, Ball, Mover, SquadZone, Rigidbody, _playerConfig, _inputController),
                new PlayerAttackState(this, CollisionHandler, Collider, Rigidbody, AnimatorController, BallHolder, TargetScanner, TargetProvider, Teammates, _inputController, BallThrower),
                new PlayerDodgeState(AnimatorController, CollisionHandler, HitDetector, Collider),
                new PlayerDeathState(AnimatorController, CollisionHandler, Collider, BallHolder, BallThrower),
            };
        }

        protected override EntityConfig GetConfig()
        {
            return _playerConfig;
        }
    }
}
