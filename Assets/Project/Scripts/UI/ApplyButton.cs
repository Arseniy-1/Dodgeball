using System;
using Project.Scripts.Services.AudioService;

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