using System;
using System.Threading.Tasks;
using UnityEngine;

public class StartGameCanvas : InteractiveCanvas
{
    public event Action OnStartGameButtonPressed;

    protected override void HandleButtonClick()
    {
        OnStartGameButtonPressed?.Invoke();
    }
}