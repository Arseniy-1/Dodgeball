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
            _exitButton.ButtonClicked += OnExitButtonClicked;
        
            foreach (var languageButton in _lanquageButtons)
            {
                languageButton.ButtonClicked += OnButtonClicked;
            }
        }

        private void OnDisable()
        {
            _exitButton.ButtonClicked -= OnExitButtonClicked;
        
            foreach (var languageButton in _lanquageButtons)
            {
                languageButton.ButtonClicked -= OnButtonClicked;
            }
        }

        [Button]
        private void OnButtonClicked(Languages language)
        {
            YG2.lang = language.ToString();
        
            YG2.SwitchLanguage(YG2.lang);
            LocalizationManager.Language = YG2.lang;
        
            YG2.SaveProgress();
        
            foreach (var languageButton in _lanquageButtons)
                languageButton.UpdateLanguage();
        }

        private void OnExitButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}