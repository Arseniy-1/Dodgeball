using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.UI.Canvases
{
    public class TutorialCanvas : GameCanvas
    {
        [SerializeField] private List<TutorialScreen> _tutorialScreens;

        private void OnEnable()
        {
            foreach (TutorialScreen tutorialScreen in _tutorialScreens)
                tutorialScreen.Initialize();
        }
    }
}