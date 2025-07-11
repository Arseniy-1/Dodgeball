using System;

public class RewardButton : ButtonHandler
{
    public event Action RewardButtonClicked;
    
    protected override void HandleButtonClick()
    {
        RewardButtonClicked?.Invoke();
        AudioID.UISoft.PlayOneShot();
    }
}