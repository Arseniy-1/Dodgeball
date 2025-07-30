using UnityEngine;

namespace Project.Scripts.UI.Canvases
{
    public class LeaderboardCanvas : GameCanvas
    {
        [SerializeField] private ExitButton _exitButton;

        private void OnEnable()
        {
            _exitButton.ExitButtonClicked += OnExitButtonClicked;
        }

        private void OnDisable()
        {
            _exitButton.ExitButtonClicked -= OnExitButtonClicked;
        }
    
        private void OnExitButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}