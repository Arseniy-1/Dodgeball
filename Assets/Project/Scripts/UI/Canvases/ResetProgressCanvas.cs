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
            _exitButton.ButtonClicked += OnExitButtonClicked;
            _applyButton.ButtonClicked += OnApplyButtonClicked;
        }

        private void OnDisable()
        {
            _exitButton.ButtonClicked -= OnExitButtonClicked;
            _applyButton.ButtonClicked -= OnApplyButtonClicked;
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