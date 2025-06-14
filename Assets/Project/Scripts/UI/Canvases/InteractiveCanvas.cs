using UnityEngine;
using UnityEngine.UI;

public abstract class InteractiveCanvas : GameCanvas
{
    [SerializeField] private Button _startGameButton;
    
    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(HandleButtonClick);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(HandleButtonClick);
    }

    protected abstract void HandleButtonClick();
}