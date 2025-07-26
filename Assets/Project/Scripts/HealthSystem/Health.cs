using System;
using Project.Scripts.Services;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.Services.CameraShakeServiceSystem;
using Project.Scripts.Services.EffectService;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.HealthSystem
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        [SerializeField] private int _currentHealthPoint;

        private CollisionHandler _collisionHandler;

        public event Action<int, int> HealthChanged;
        public event Action LostHealth;

        private void OnEnable()
        {
            HealthChanged?.Invoke(_currentHealthPoint, _maxHealth);
        }

        public void Initialize(CollisionHandler collisionHandler)
        {
            _collisionHandler = collisionHandler;
            _collisionHandler.DamageTaken += TakeDamage;
        }

        [Button]
        public void Heal(int amount)
        {
            if (amount <= 0)
                return;

            _currentHealthPoint = Mathf.Clamp(_currentHealthPoint + amount, 0, _maxHealth);

            HealthChanged?.Invoke(_currentHealthPoint, _maxHealth);
        }

        [Button]
        public void TakeDamage(int amount)
        {
            if (amount <= 0)
                return;
        
            if (_currentHealthPoint <= 0)
                return;

            _currentHealthPoint = Mathf.Clamp(_currentHealthPoint - amount, 0, _maxHealth);

            HealthChanged?.Invoke(_currentHealthPoint, _maxHealth);
            ShakeID.Light.Play();

            if (_currentHealthPoint == 0)
            {
                LostHealth?.Invoke();

                return;
            }

            EffectID.Cry.PlayEffect(transform);
            AudioID.DamageTaken.PlayOneShot();
        }

        public void Reset()
        {
            _currentHealthPoint = _maxHealth;
            _collisionHandler.DamageTaken -= TakeDamage;
        }
    }
}