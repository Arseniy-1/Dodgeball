using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Services.Ball
{
    public class BallThrowerView : MonoBehaviour
    {
        [SerializeField] private BallThrower _ballThrower;
        [SerializeField] private Image _chargeFill;
        [SerializeField] private Image _chargeBackground;
    
        [SerializeField] private Color _minChargeColor = Color.green;
        [SerializeField] private Color _maxChargeColor = Color.red;

        private void OnEnable()
        {
            _ballThrower.Charging += OnCharging;
            _ballThrower.Thrown += OnThrown;
            HideView();
        }

        private void OnDisable()
        {
            _ballThrower.Charging -= OnCharging;
            _ballThrower.Thrown -= OnThrown;
        }

        private void Show(float minForce, float maxForce, float currentForce)
        {
            _chargeFill.enabled = true;
            _chargeBackground.enabled = true;

            float chargePercent = Mathf.InverseLerp(minForce, maxForce, currentForce);
            _chargeFill.fillAmount = chargePercent;

            _chargeFill.color = Color.Lerp(_minChargeColor, _maxChargeColor, chargePercent);
        }
        
        private void HideView()
        {
            _chargeFill.enabled = false;
            _chargeBackground.enabled = false;
        }

        private void OnCharging(float minForce, float maxForce, float currentForce)
        {
            Show(minForce, maxForce, currentForce);
        }
        
        private void OnThrown()
        {
            HideView();
        }
    }
}