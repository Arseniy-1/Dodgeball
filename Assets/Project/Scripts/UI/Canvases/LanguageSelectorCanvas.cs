using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using Project.Scripts.Settings;
using Sirenix.OdinInspector;
using UnityEngine;
using YG;

namespace Project.Scripts.UI.Canvases
{
    public class LanguageSelectorCanvas : GameCanvas
    {
        [SerializeField] private List<LanguageButton> _lanquageButtons;
        [SerializeField] private ExitButton _exitButton;
    
        private void OnEnable()
        {
            _exitButton.ExitButtonClicked += Disable;
        
            foreach (var languageButton in _lanquageButtons)
            {
                languageButton.ButtonClicked += HandleButtonButtonClicked;
            }
        }

        private void OnDisable()
        {
            _exitButton.ExitButtonClicked -= Disable;
        
            foreach (var languageButton in _lanquageButtons)
            {
                languageButton.ButtonClicked -= HandleButtonButtonClicked;
            }
        }

        [Button]
        private void HandleButtonButtonClicked(Languages language)
        {
            YG2.lang = language.ToString();
        
            YG2.SwitchLanguage(YG2.lang);
            LocalizationManager.Language = YG2.lang;
        
            YG2.SaveProgress();
        
            foreach (var languageButton in _lanquageButtons)
                languageButton.UpdateLanguage();
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}