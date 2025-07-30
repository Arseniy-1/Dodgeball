using UnityEngine;
using YG;

namespace Project.Scripts.UI.Canvases
{
    public class ResetProgressCanvas : GameCanvas
    {
        [SerializeField] private ExitButton _exitButton;
        [SerializeField] private ApplyButton _applyButton;
        [SerializeField] private SavesSystem.Saves _saves;
    
        private void OnEnable()
        {
            _exitButton.ExitButtonClicked += OnExitButtonClicked;
            _applyButton.ApplyButtonClicked += OnApplyButtonClicked;
        }

        private void OnDisable()
        {
            _exitButton.ExitButtonClicked -= OnExitButtonClicked;
            _applyButton.ApplyButtonClicked -= OnApplyButtonClicked;
        }

        private void OnExitButtonClicked()
        {
            gameObject.SetActive(false);
        }   
    
        private void OnApplyButtonClicked()
        {
            _saves.ResetProgress();
            YG2.SaveProgress();
        
            OnExitButtonClicked();
        }
    }
}