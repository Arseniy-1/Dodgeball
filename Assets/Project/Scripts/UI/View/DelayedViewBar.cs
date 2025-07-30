using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public abstract class DelayedViewBar : ViewBar
    {
        [SerializeField] private Slider _delayedBar;
        [SerializeField] private float _delaySpeed = 2f;

        private void Update()
        {
            _delayedBar.value = _delayedBar.value > TargetFill
                ? Mathf.MoveTowards(_delayedBar.value, TargetFill, Time.deltaTime * _delaySpeed)
                : TargetFill;
        }

        protected override void Reset()
        {
            base.Reset();
            _delayedBar.value = TargetFill;
        }
    }
}