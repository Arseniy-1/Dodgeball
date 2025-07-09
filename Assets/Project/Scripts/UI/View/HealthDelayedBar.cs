using UnityEngine;

public class HealthDelayedBar : DelayedViewBar
{
    [SerializeField] private Health _health;

    private void OnEnable()
    {
        _health.HealthChanged += OnValueChanged;
        Reset();
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnValueChanged;
    }
}