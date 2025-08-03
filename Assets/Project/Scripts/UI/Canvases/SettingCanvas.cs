using UnityEngine;

namespace Project.Scripts.UI.Canvases
{
    public class SettingCanvas : GameCanvas
    {
        [SerializeField] private ExitButton _exitButton;

        private void OnEnable()
        {
            _exitButton.ButtonClicked += OnExitButtonClicked;
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            _exitButton.ButtonClicked -= OnExitButtonClicked;
            Time.timeScale = 1;
        }

        private void OnExitButtonClicked()
        {
            gameObject.SetActive(false);
        }
    }
}