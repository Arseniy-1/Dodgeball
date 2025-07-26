using System;
using Project.Scripts.Services.AudioServiceSystem;

namespace Project.Scripts.UI
{
    public class ExitButton : ButtonHandler
    {
        public event Action ExitButtonClicked;
    
        protected override void HandleButtonClick()
        {
            ExitButtonClicked?.Invoke();
            AudioID.NegativeAction.PlayOneShot();
        }
    }
}