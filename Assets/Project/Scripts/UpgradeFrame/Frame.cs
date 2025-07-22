using System;
using System.Collections.Generic;
using Project.Scripts.Services.Ball;
using Project.Scripts.UpgradeFrame.BallUpdaters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.Scripts.UpgradeFrame
{
    public class Frame : MonoBehaviour
    {
        [SerializeField] private FrameView _frameView;
        [SerializeField] private Collider _collider;

        [SerializeField] private List<Transform> _waypoints;
        [SerializeField] private float _speed = 3;

        private BallUpgrade _ballUpgrade;

        private int _currentWaypoint = 0;

        public event Action<Frame> OnFrameHit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Chargeable chargeable))
            {
                if (chargeable.IsCharged == false)
                    return;
            
                if (chargeable.TryGetComponent(out Ball ball))
                {
                    _ballUpgrade.UpgradeBall(ball);
                    HandleBallHit();
                }
            }
        }

        private void Update()
        {
            if (transform.position == _waypoints[_currentWaypoint].position)
                _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Count;

            transform.position = Vector3.MoveTowards(
                transform.position, 
                _waypoints[_currentWaypoint].position,
                _speed * Time.deltaTime);
        }

        public void Activate(BallUpgrade ballUpgrade)
        {
            _collider.enabled = true;
            _frameView.gameObject.SetActive(transform);

            _ballUpgrade = ballUpgrade;
            _frameView.Initialize(_ballUpgrade.BallUpgradeInfo);
        }

        [Button]
        private void HandleBallHit()
        {
            _collider.enabled = false;
            _frameView.gameObject.SetActive(false);

            OnFrameHit?.Invoke(this);
        }
    }
}