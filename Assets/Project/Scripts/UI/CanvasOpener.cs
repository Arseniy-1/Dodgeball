using Project.Scripts.Services.AudioService;
using Project.Scripts.UI.Canvases;
using UnityEngine;

namespace Project.Scripts.UI
{
    public class CanvasOpener : ButtonHandler
    {
        [SerializeField] private GameCanvas _canvas;
    
        protected override void HandleButtonClick()
        {
            _canvas.gameObject.SetActive(true);
            AudioID.UISoft.PlayOneShot();
        }
    }
}