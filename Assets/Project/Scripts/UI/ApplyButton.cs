using System;

public class ApplyButton : ButtonHandler
{
    public event Action ApplyButtonClicked;
    
    protected override void HandleButtonClick()
    {
        ApplyButtonClicked?.Invoke();
        AudioID.UISoft.PlayOneShot();
    }
}