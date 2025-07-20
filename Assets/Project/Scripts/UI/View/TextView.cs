using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public abstract class TextView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textView;

        protected void OnValueChanged(int current, int max)
        {
            if (current >= max)
                current = 0;
        
            _textView.text = current + "/" + max;
        }
    }
}