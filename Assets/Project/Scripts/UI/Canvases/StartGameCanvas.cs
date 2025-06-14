using System;
using UnityEngine;

public class StartGameCanvas : InteractiveCanvas
{
    public event Action OnStartGameButtonPressed;

    protected override void HandleButtonClick()
    {
        OnStartGameButtonPressed?.Invoke();
    }
}