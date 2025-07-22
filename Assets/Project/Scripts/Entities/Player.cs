using System;
using System.Collections.Generic;
using Project.Scripts.Services;
using Project.Scripts.Services.AudioService;
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

        private List<IState> _playerStates = new ();

        public event Action<Player> OnDestroyed;

        public override void Initialize(Collider squadZone, List<Entity> teammates, Ball ball)
        {
            base.Initialize(squadZone, teammates, ball);
            BallThrower.Initialize(_playerConfig);

            foreach (var state in _playerStates)
            {
                if (state is IDisposable disposable)
                    disposable.Dispose();
            }
        
            _playerStates.Clear();

            _playerStates = new List<IState>
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

            CreateStateMachine(_playerStates);

            foreach (var state in _playerStates)
                state.Initialize(StateMachine);

            Reset();
        }
    
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
        protected override async void HandleLostHealth()
        {
            StateMachine.SwitchState<PlayerDeathState>();
            HealthCanvas.gameObject.SetActive(false);
            EffectID.Death.PlayEffect(transform);
            AudioID.Dead.PlayOneShot();
        
            await AnimatorController.Death();
            await HideEntity();

            Die();
        }
    }
}
