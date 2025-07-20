using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Canvases
{
    public abstract class TutorialScreen : GameCanvas
    {
        [SerializeField] protected Image SelectionCircle;
        [SerializeField] protected ApplyButton ApplyButton;
    
        public abstract void Initialize();
    }
}