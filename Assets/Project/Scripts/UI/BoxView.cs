using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class BoxView : MonoBehaviour
    {
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

        private void SetupStaticAnimation()
        {
            _staticAnimation?.Kill();

            _staticAnimation = DOTween.Sequence()
                .Append(_rewardBoxImage.transform.DOScale(_originalScale * 1.1f, 0.5f))
                .Append(_rewardBoxImage.transform.DOScale(_originalScale, 0.5f))
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        public async UniTask ShowBoxAnimation(float duration, float endScale, float whiteFadeDuration)
        {
            _staticAnimation.Complete();

            _burstAnimation = DOTween.Sequence()
                .Append(_rewardBoxImage.transform.DOShakeScale(duration, 0.3f, 10))
                .Join(_rewardBoxImage.transform.DOShakeRotation(duration, 15f, 10))
                .Append(_rewardBoxImage.transform.DOScale(endScale, whiteFadeDuration))
                .Join(_rewardBoxImage.DOColor(Color.black, whiteFadeDuration))
                .SetEase(Ease.InOutQuad);

            await _burstAnimation.AsyncWaitForCompletion();
        }

        private void ResetBox()
        {
            _rewardBoxImage.transform.localScale = _originalScale;
            _rewardBoxImage.transform.rotation = Quaternion.identity;
            _rewardBoxImage.color = Color.white;
        }
    }
}