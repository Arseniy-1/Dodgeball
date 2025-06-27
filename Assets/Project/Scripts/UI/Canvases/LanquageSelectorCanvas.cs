using UnityEngine;
using System.Collections.Generic;
using YG;

public class LanquageSelectorCanvas : GameCanvas
{
    [SerializeField] private List<LanquageButton> _lanquageButtons;
     [SerializeField] private ExitButton _exitButton;
    
    private void OnEnable()
    {
        _exitButton.ExitButtonClicked += Disable;
        
        foreach (var lanquageButton in _lanquageButtons)
        {
            lanquageButton.OnClick += HanldeButtonClick;
        }
    }

    private void OnDisable()
    {
        _exitButton.ExitButtonClicked -= Disable;
        
        foreach (var lanquageButton in _lanquageButtons)
        {
            lanquageButton.OnClick -= HanldeButtonClick;
        }
    }

    private void HanldeButtonClick(Lanquages lanquage)
    {
        // YandexGame.savesData.language = lanquage.ToString();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}