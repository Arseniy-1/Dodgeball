using UnityEngine;
using UnityEngine.UI;
using YG;

public abstract class SettingToggle : ButtonHandler
{
    [SerializeField] private Sprite _enabledSprite;
    [SerializeField] private Sprite _disabledSprite;

    [SerializeField] private Image _view;

    public virtual void Initialize()
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
        Toggle();
        YandexGame.SaveProgress();
        EnableSetting();

        UpdateView();
    }

    protected abstract bool IsEnabled();
    protected abstract void Toggle();
    protected abstract void EnableSetting();
}