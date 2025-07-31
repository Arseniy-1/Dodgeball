using System;
using Project.Scripts.Services.AudioServiceSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public abstract class ButtonHandler : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private AudioID _sound;
        
        public event Action ButtonClicked;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(HandleButtonClick);
        }

        protected virtual void HandleButtonClick()
        {
            ButtonClicked?.Invoke();
            _sound.PlayOneShot();
        }
    }
}