using UnityEngine;
using YG;

public class ResetProgressCanvas : GameCanvas
{
    [SerializeField] private ExitButton _exitButton;
    [SerializeField] private ApplyButton _applyButton;
    [SerializeField] private Saves _saves;
    
    private void OnEnable()
    {
        _exitButton.ExitButtonClicked += Disable;
        _applyButton.ApplyButtonClicked += ResetProgress;
    }

    private void OnDisable()
    {
        _exitButton.ExitButtonClicked -= Disable;
        _applyButton.ApplyButtonClicked -= ResetProgress;
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }   
    
    private void ResetProgress()
    {
        _saves.ResetProgress();
        YG2.SaveProgress();
        
        Disable();
    }
}