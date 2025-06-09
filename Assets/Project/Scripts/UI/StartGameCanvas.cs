using System;
using UnityEngine;
using UnityEngine.UI;

public class StartGameCanvas : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;

    public event Action OnStartGameButtonPressed;
    
    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(HandleButtonClick);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        OnStartGameButtonPressed?.Invoke();       
    }
}