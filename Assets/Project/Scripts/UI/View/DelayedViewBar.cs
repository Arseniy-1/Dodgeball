using UnityEngine;
using UnityEngine.UI;

public abstract class DelayedViewBar : ViewBar
{
    [SerializeField] private Slider _delayedBar;
    [SerializeField] private float _delaySpeed = 2f;

    private void Update()
    {
        if (_delayedBar.value > TargetFill)
            _delayedBar.value = Mathf.MoveTowards(_delayedBar.value, TargetFill, Time.deltaTime * _delaySpeed);
        else
            _delayedBar.value = TargetFill;
    }

    protected override void Reset()
    {
        base.Reset();
        _delayedBar.value = TargetFill;
    }
}