using TMPro;
using UnityEngine;

namespace Project.Scripts.UI.View
{
    public class TextView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textView;

        public void UpdateView(int current, int max)
        {
            OnValueChanged(current, max);
        }
        
        private void OnValueChanged(int current, int max)
        {
            if (current >= max)
                current = 0;
        
            _textView.text = current + "/" + max;
        }
    }
}