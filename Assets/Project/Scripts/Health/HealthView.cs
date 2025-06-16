using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Slider _mainBar;
    [SerializeField] private Slider _delayedBar;
    [SerializeField] private float _delaySpeed = 2f;
    [SerializeField] private Health _health;

    private float _targetFill;

    private void OnEnable()
    {
        _health.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnHealthChanged;
    }

    private void Update()
    {
        if (_delayedBar.value > _targetFill)
            _delayedBar.value = Mathf.MoveTowards(_delayedBar.value, _targetFill, Time.deltaTime * _delaySpeed);
        else
            _delayedBar.value = _targetFill;
    }

    private void OnHealthChanged(float current, float max)
    {
        _targetFill = current / max;
        _mainBar.value = _targetFill;
    }
}