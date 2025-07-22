using DG.Tweening;
using Project.Scripts.Messages;
using UniRx;
using UnityEngine;

namespace Project.Scripts.Services.CameraShakeService
{
    public class CameraShakeService : MonoBehaviour
    {
        private readonly CompositeDisposable _disposable = new ();
        
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraShakeSettings _settings;

        private Transform _cameraTransform;

        private Vector3 _cameraOriginalPosition;
        private Tween _shakeTween;

        private void OnEnable()
        {
            _cameraTransform = _camera.transform;

            _cameraOriginalPosition = _cameraTransform.localPosition;

            MessageBrokerHolder.GameActions
                .Receive<M_CameraShake>()
                .Subscribe(message => Shake(message.ShakeID))
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable?.Clear();
            _shakeTween?.Kill();
        }

        private void Shake(ShakeID shakeID)
        {
            if (_settings.TryGet(shakeID, out CameraShakeData shake) == false)
                return;

            _shakeTween?.Kill();

            _shakeTween = _camera.transform
                .DOShakePosition(shake.Duration, shake.Strength, shake.Vibrato, shake.Randomness)
                .OnKill(() => _cameraTransform.localPosition = _cameraOriginalPosition);
        }
    }
}