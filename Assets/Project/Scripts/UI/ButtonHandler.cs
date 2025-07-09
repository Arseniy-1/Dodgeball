using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonHandler : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private AudioID _sound;

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleButtonClick);
    }

    protected abstract void HandleButtonClick();
}