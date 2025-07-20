using System;

namespace Project.Scripts.UI.Canvases
{
    public class StartGameCanvas : InteractiveCanvas
    {
        public event Action OnStartGameButtonPressed;

        protected override void HandleButtonClick()
        {
            OnStartGameButtonPressed?.Invoke();
        }
    }
}