using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class LanquageButton : ButtonHandler
{
    [SerializeField] private LanguageData _languageData;
    
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _view;
    
    public event Action<Lanquages> OnClick;

    private void OnEnable()
    {
        _name.text = _languageData.Name;
        _view.sprite = _languageData.View;
    }
    
    protected override void HandleButtonClick()
    {
        OnClick?.Invoke(_languageData.Lanquage);
    }
}