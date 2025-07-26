using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class BoxView : MonoBehaviour
    {
        private const float StaticAnimationScaleFactor = 1.1f;
        private const float StaticAnimationDuration = 0.5f;
        private const Ease StaticAnimationEase = Ease.InOutSine;
        
        private const float BurstShakeStrength = 0.3f;
        private const int BurstShakeVibrato = 10;
        private const float BurstRotationStrength = 15f;
        private const Ease BurstAnimationEase = Ease.InOutQuad;
        
        [SerializeField] private Image _rewardBoxImage;

        private Sequence _staticAnimation;
        private Sequence _burstAnimation;
        private Vector3 _originalScale;

        public void Enable()
        {
            gameObject.SetActive(true);
            _originalScale = Vector3.one;
            SetupStaticAnimation();
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            ResetBox();
            _staticAnimation?.Kill();
            _burstAnimation?.Kill();
        }

        public async UniTask ShowBoxAnimation(float duration, float endScale, float whiteFadeDuration)
        {
            _staticAnimation.Complete();

            _burstAnimation = DOTween.Sequence()
                .Append(_rewardBoxImage.transform.DOShakeScale(duration, BurstShakeStrength, BurstShakeVibrato))
                .Join(_rewardBoxImage.transform.DOShakeRotation(duration, BurstRotationStrength, BurstShakeVibrato))
                .Append(_rewardBoxImage.transform.DOScale(endScale, whiteFadeDuration))
                .Join(_rewardBoxImage.DOColor(Color.black, whiteFadeDuration))
                .SetEase(BurstAnimationEase);

            await _burstAnimation.AsyncWaitForCompletion();
        }
 
        private void SetupStaticAnimation()
        {
            _staticAnimation?.Kill();

            _staticAnimation = DOTween.Sequence()
                .Append(_rewardBoxImage.transform.DOScale(_originalScale * StaticAnimationScaleFactor, StaticAnimationDuration))
                .Append(_rewardBoxImage.transform.DOScale(_originalScale, StaticAnimationDuration))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(StaticAnimationEase);
        }
        
        private void ResetBox()
        {
            _rewardBoxImage.transform.localScale = _originalScale;
            _rewardBoxImage.transform.rotation = Quaternion.identity;
            _rewardBoxImage.color = Color.white;
        }
    }
}