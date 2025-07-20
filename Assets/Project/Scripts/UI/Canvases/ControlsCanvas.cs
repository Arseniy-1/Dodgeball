using UnityEngine;

namespace Project.Scripts.UI.Canvases
{
    public class ControlsCanvas : GameCanvas
    {
        [SerializeField] private ExitButton _exitButton;

        private void OnEnable()
        {
            _exitButton.ExitButtonClicked += Disable;
        }

        private void OnDisable()
        {
            _exitButton.ExitButtonClicked -= Disable;
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}