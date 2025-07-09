using System;

public class ExitButton : ButtonHandler
{
    public event Action ExitButtonClicked;
    
    protected override void HandleButtonClick()
    {
        ExitButtonClicked?.Invoke();
        AudioID.NegativeAction.PlayOneShot();
    }
}