using UnityEngine;

public class LeaderboardCanvas : GameCanvas
{
    [SerializeField] private ExitButton _exitButton;

    private void OnEnable()
    {
        _exitButton.ExitButtonClicked += Disable;
    }

    private void OnDisable()
    {
        _exitButton.ExitButtonClicked -= Disable;
    }
    
    private void Disable()
    {
        gameObject.SetActive(false);
    }
}