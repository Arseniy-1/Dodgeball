using Project.Scripts.Services.AudioServiceSystem;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Project.Scripts.UI
{
    public abstract class SettingToggle : ButtonHandler
    {
        [SerializeField] private Sprite _enabledSprite;
        [SerializeField] private Sprite _disabledSprite;

        [SerializeField] private Image _view;

        public virtual void Initialize()
        {
            UpdateView();
        }

        protected override void HandleButtonClick()
        {
            Toggle();
            YG2.SaveProgress();
            UpdateView();
        }

        protected virtual void Toggle()
        {
            AudioID.UISoft.PlayOneShot();
        }
    
        protected abstract bool IsEnabled();
        
        private void UpdateView()
        {
            _view.sprite = IsEnabled() ? _enabledSprite : _disabledSprite;
        }
    }
}