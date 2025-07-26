using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class HealthDelayedBar : DelayedViewBar
    {
        [SerializeField] private HealthSystem.Health _health;

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
}