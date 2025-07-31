using UnityEngine;

namespace Project.Scripts.UI.Canvases
{
    public class ControlsCanvas : GameCanvas
    {
        [SerializeField] private ExitButton _exitButton;

        private void OnEnable()
        {
            _exitButton.ButtonClicked += OnExitButtonClicked;
        }

        private void OnDisable()
        {
            _exitButton.ButtonClicked -= OnExitButtonClicked;
        }

        private void OnExitButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}