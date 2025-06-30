using UnityEngine;
using UnityEngine.UI;

public abstract class InteractiveCanvas : GameCanvas
{
    [SerializeField] private Button _button;
    
    protected virtual void OnEnable()
    {
        EnableButton();
        _button.onClick.AddListener(HandleButtonClick);
    }

    protected virtual void OnDisable()
    {
        _button.onClick.RemoveListener(HandleButtonClick);
    }

    protected void DisableButton() => _button.interactable = false;

    protected void EnableButton() => _button.interactable = true;

    protected abstract void HandleButtonClick();
}