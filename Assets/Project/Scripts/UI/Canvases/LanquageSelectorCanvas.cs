using UnityEngine;
using System.Collections.Generic;
using Assets.SimpleLocalization.Scripts;
using Sirenix.OdinInspector;
using YG;

public class LanquageSelectorCanvas : GameCanvas
{
    [SerializeField] private List<LanquageButton> _lanquageButtons;
    [SerializeField] private ExitButton _exitButton;
    
    private void OnEnable()
    {
        _exitButton.ExitButtonClicked += Disable;
        
        foreach (var languageButton in _lanquageButtons)
        {
            languageButton.OnClick += HandleButtonClick;
        }
    }

    private void OnDisable()
    {
        _exitButton.ExitButtonClicked -= Disable;
        
        foreach (var languageButton in _lanquageButtons)
        {
            languageButton.OnClick -= HandleButtonClick;
        }
    }

    [Button]
    private void HandleButtonClick(Languages language)
    {
        YG2.saves.SettingsData.Language = language;
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