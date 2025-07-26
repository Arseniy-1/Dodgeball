using System;
using Project.Scripts.Services.Ball;
using UnityEngine;

namespace Project.Scripts.Services
{
    public class HitDetector : MonoBehaviour
    {
        public event Action DetectBallHit;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Chargeable chargeable))
            {
                if (chargeable.IsCharged)
                {
                    DetectBallHit?.Invoke();
                }
            }
        }
    }
}