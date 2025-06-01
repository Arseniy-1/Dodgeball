using UnityEngine;
using UnityEngine.UI;

public class BallThrowerView : MonoBehaviour
{
    [SerializeField] private BallThrower _ballThrower;
    [SerializeField] private Image _chargeFill;
    [SerializeField] private Image _chargeBackground;

    private void OnEnable()
    {
        _ballThrower.OnCharging += Show;
        _ballThrower.OnThrown += HideView;
        HideView();
    }

    private void OnDisable()
    {
        _ballThrower.OnCharging -= Show;
        _ballThrower.OnThrown -= HideView;
    }

    private void Show(float minForce, float maxForce, float currentForce)
    {
        _chargeFill.enabled = true;
        _chargeBackground.enabled = true;

        float chargePercent = Mathf.InverseLerp(minForce, maxForce, currentForce);
        _chargeFill.fillAmount = chargePercent;

        _chargeFill.color = Color.Lerp(Color.green, Color.red, chargePercent);
    }


    private void HideView()
    {
        _chargeFill.enabled = false;
        _chargeBackground.enabled = false;
    }
}