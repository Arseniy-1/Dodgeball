using DG.Tweening;
using Project.Scripts.Services;
using UnityEngine;

namespace Project.Scripts
{
    public class TweenAnimatorExample : MonoBehaviour
    {
        private const float TargetXPosition = 10f;
        private const float AnimationDuration = 1.5f;

        [SerializeField] private Transform _transform;
        [SerializeField] private PlayerInputController _playerInputController;

        private void OnEnable()
        {
            _playerInputController.ActionButtonStarted += Do;
        }

        private void OnDisable()
        {
            _playerInputController.ActionButtonStarted -= Do;
        }

        private void Do()
        {
            _transform.DOMoveX(TargetXPosition, AnimationDuration).From();
        }
    }
}