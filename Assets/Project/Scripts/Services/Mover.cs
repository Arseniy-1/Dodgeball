using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services.AudioServiceSystem;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class Mover : MonoBehaviour
    {
        private float _stoppingDistance = 0.1f;

        private float _stepDistanceThreshold = 1.2f;
        private float _distanceSinceLastStep = 0f;
        private Vector3 _lastPosition;

        public async UniTask MoveTo(Vector3 target, float speed, CancellationToken cancellationToken)
        {
            _lastPosition = transform.position;
            _distanceSinceLastStep = 0f;

            while (this != null && cancellationToken.IsCancellationRequested == false &&
                   Vector3.Distance(transform.position, target) > _stoppingDistance)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Vector3 direction = target - transform.position;
                direction.y = 0;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    transform.position + direction,
                    speed * Time.deltaTime);

                TrackStep();
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }

        public void FollowTarget(Transform target, float speed)
        {
            if (target == null)
                return;

            Vector3 currentPos = transform.position;
            Vector3 targetPos = target.position;
            targetPos.y = currentPos.y;

            transform.position = Vector3.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);

            TrackStep();
        }

        private void TrackStep()
        {
            float moved = Vector3.Distance(transform.position, _lastPosition);
            float minStepDistance = 0.001f;

            _distanceSinceLastStep += moved;

            if (moved > minStepDistance && _distanceSinceLastStep >= _stepDistanceThreshold)
            {
                AudioID.Walk.PlayOneShot();
                _distanceSinceLastStep = 0f;
            }

            _lastPosition = transform.position;
        }
    }
}