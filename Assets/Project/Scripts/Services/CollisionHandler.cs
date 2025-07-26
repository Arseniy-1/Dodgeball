using System;
using Project.Scripts.Entities;
using Project.Scripts.Services.Ball;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class CollisionHandler : MonoBehaviour
    {
        public event Action<Scripts.Ball> BallDetected;
        public event Action<int> DamageTaken;

        private void OnCollisionEnter(Collision collision)
        {
            if (enabled == false)
                return;

            if (collision.collider.TryGetComponent(out Chargeable chargeable))
            {
                if (chargeable.IsCharged)
                {
                    if (chargeable.TryGetComponent(out Damageable damageable))
                    {
                        DamageTaken?.Invoke(damageable.Damage);

                        return;
                    }
                }
            }

            if (collision.collider.TryGetComponent(out Scripts.Ball interactable))
                InteractWithBall(interactable);
        }

        private void OnCollisionStay(Collision other)
        {
            if (other.collider.TryGetComponent(out Scripts.Ball interactable))
                InteractWithBall(interactable);
        }

        private void InteractWithBall(Scripts.Ball ball)
        {
            if (GameStatusService.Instance.IsBallFree == false)
                return;

            BallDetected?.Invoke(ball);
        }
    }
}