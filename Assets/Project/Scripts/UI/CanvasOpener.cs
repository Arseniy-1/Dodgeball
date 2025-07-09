using UnityEngine;

public class CanvasOpener : ButtonHandler
{
    [SerializeField] private GameCanvas _canvas;
    
    protected override void HandleButtonClick()
    {
        _canvas.gameObject.SetActive(true);
        AudioID.UISoft.PlayOneShot();
    }
}