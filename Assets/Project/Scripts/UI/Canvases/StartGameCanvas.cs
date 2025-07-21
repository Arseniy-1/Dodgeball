using System;

namespace Project.Scripts.UI.Canvases
{
    public class StartGameCanvas : InteractiveCanvas
    {
        public event Action StartGameButtonPressed;

        protected override void HandleButtonClick()
        {
            StartGameButtonPressed?.Invoke();
        }
    }
}