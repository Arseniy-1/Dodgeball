using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealthPoint;
    
    private CollisionHandler _collisionHandler;
    
    public event Action<float, float> HealthChanged;
    public event Action LostHealth;

    private void OnEnable()
    {
        HealthChanged?.Invoke(_currentHealthPoint, _maxHealth);
    }

    private void OnDestroy()
    {
        _collisionHandler.DamageTaken -= TakeDamage;
    }

    public void Initialize(CollisionHandler collisionHandler)
    {
        _collisionHandler = collisionHandler;
        _collisionHandler.DamageTaken += TakeDamage;
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
            return;

        _currentHealthPoint = Mathf.Clamp(_currentHealthPoint + amount, 0, _maxHealth);

        HealthChanged?.Invoke(_currentHealthPoint, _maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
            return;

        _currentHealthPoint = Mathf.Clamp(_currentHealthPoint - amount, 0, _maxHealth);

        if (_currentHealthPoint == 0)
            LostHealth?.Invoke();

        HealthChanged?.Invoke(_currentHealthPoint, _maxHealth);
    }

    public void Reset()
    {
        _currentHealthPoint = _maxHealth;
    }
}