using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Canvases
{
    public abstract class InteractiveCanvas : GameCanvas
    {
        [SerializeField] private Button _button;
    
        protected virtual void OnEnable()
        {
            EnableButton();
            _button.onClick.AddListener(HandleButtonClick);
        }

        protected virtual void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        protected void DisableButton() => _button.interactable = false;

        protected abstract void HandleButtonClick();

        private void EnableButton() => _button.interactable = true;
    }
}