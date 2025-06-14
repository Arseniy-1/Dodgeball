using UnityEngine;
using UnityEngine.UI;
using YG;

public abstract class SettingToggle : ButtonHandler
{
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;

    [SerializeField] private Image _view;

    private void Start()
    {
        UpdateView();
    }

    private void UpdateView()
    {
        if (IsEnabled())
        {
            _view.sprite = _enabledSprite;
        }
        else
        {
            _view.sprite = _disabledSprite;
        }
    }

    protected override void HandleButtonClick()
    {
        EnableSetting();
        YandexGame.SaveProgress();

        UpdateView();
    }

    protected abstract bool IsEnabled();
    
    protected abstract void EnableSetting();
}