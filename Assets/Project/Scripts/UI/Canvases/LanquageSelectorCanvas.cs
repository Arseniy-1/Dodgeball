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
    private void HandleButtonClick(Lanquages language)
    {
        YG2.lang = language.ToString();
        YG2.SaveProgress();
        
        YG2.SwitchLanguage(YG2.lang);
        LocalizationManager.Language = YG2.lang;
        
        foreach (var languageButton in _lanquageButtons)
            languageButton.UpdateLanguage();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}