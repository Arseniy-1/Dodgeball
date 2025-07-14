using UnityEngine;

public class SettingCanvas : GameCanvas
{
    [SerializeField] private ExitButton _exitButton;

    private void OnEnable()
    {
        _exitButton.ExitButtonClicked += Disable;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        _exitButton.ExitButtonClicked -= Disable;
        Time.timeScale = 1;
    }
    
    private void Disable()
    {
        gameObject.SetActive(false);
    }
}