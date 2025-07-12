using UnityEngine;
using System;
using Sirenix.OdinInspector;

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

    private void OnDestroy()
    {
        _collisionHandler.DamageTaken -= TakeDamage;
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
    }
}