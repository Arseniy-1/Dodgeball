using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Canvases
{
    public abstract class TutorialScreen : GameCanvas
    {
        [field: SerializeField] protected Image SelectionCircle { get; private set; }
        [field: SerializeField] protected ApplyButton ApplyButton { get; private set; }

        public abstract void Initialize();
    }
}