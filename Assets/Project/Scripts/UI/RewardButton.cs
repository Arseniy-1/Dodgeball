using System;
using Project.Scripts.Services.AudioService;

namespace Project.Scripts.UI
{
    public class RewardButton : ButtonHandler
    {
        public event Action RewardButtonClicked;
    
        protected override void HandleButtonClick()
        {
            RewardButtonClicked?.Invoke();
            AudioID.UISoft.PlayOneShot();
        }
    }
}