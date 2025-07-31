using System.Collections.Generic;
using Project.Scripts.UpgradeFrame;
using Project.Scripts.UpgradeFrame.BallUpdaters;
using UnityEngine;

namespace Project.Scripts.GameSystem
{
    public class MatchManager : MonoBehaviour
    {
        [SerializeField] private float _minInactiveInterval = 1f;
        [SerializeField] private float _maxInactiveInterval = 3f;
        [SerializeField] private List<Squad> _squads;
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private BallUpgraderFabric _ballUpgraderFabric;
        [SerializeField] private List<Frame> _frames;

        private SquadDeathHandler _deathHandler;
        private BallUpgradeHolder _ballUpgradeHolder;
        private FrameActivator _frameActivator;

        public List<Squad> Squads => _squads;

        private void Awake()
        {
            _deathHandler = new SquadDeathHandler(_squads);
            _ballUpgradeHolder = new BallUpgradeHolder(_ballUpgraderFabric);
            _frameActivator = new FrameActivator(_frames, _minInactiveInterval, _maxInactiveInterval);
        }

        private void OnDestroy()
        {
            _frameActivator.Dispose();
        }

        public void Initialize(Ball ball)
        {
            ball.transform.position = _ballPosition.position;
            _frameActivator.Initialize(_ballUpgradeHolder.Upgraders);
            _frameActivator.Activate();
        }
    }
}