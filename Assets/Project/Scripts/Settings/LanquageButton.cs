using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.SimpleLocalization.Scripts;
using TMPro;

public class LanquageButton : ButtonHandler
{
    [SerializeField] private LanguageData _languageData;
    
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _view;
    
    public event Action<Lanquages> OnClick;

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
        OnClick?.Invoke(_languageData.Language);
        AudioID.UISolid.PlayOneShot();
    }
}