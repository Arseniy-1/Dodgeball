using System;
using Assets.SimpleLocalization.Scripts;
using Project.Scripts.Services.AudioServiceSystem;
using Project.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Settings
{
    public class LanguageButton : ButtonHandler
    {
        [SerializeField] private LanguageData _languageData;
    
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _view;
    
        public event Action<Languages> ButtonClicked;

        private void Start()
        {
            UpdateLanguage();
            _view.sprite = _languageData.View;
        }

        public void UpdateLanguage()
        {
            _name.text = LocalizationManager.Localize(_languageData.Language.ToString());
        }
    
        protected override void HandleButtonClick()
        {
            ButtonClicked?.Invoke(_languageData.Language);
            AudioID.UISolid.PlayOneShot();
        }
    }
}