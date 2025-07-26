using System;
using Project.Scripts.Services.AudioServiceSystem;

namespace Project.Scripts.UI
{
    public class ApplyButton : ButtonHandler
    {
        public event Action ApplyButtonClicked;
    
        protected override void HandleButtonClick()
        {
            ApplyButtonClicked?.Invoke();
            AudioID.UISoft.PlayOneShot();
        }
    }
}