using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.View
{
    public abstract class ViewBar : MonoBehaviour
    {
        [SerializeField] private Slider _bar;

        protected float TargetFill { get; private set; }

        protected void OnValueChanged(int current, int max)
        {
            if (current >= max)
                current = 0;
        
            TargetFill = (float)current / max;
            _bar.value = TargetFill;
        }
    
        protected virtual void Reset()
        {
            _bar.value = 1;
        }
    }
}