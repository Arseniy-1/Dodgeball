using System;
using System.Collections.Generic;
using Project.Scripts.Services.EffectServiceSystem;
using Project.Scripts.UpgradeFrame;
using Project.Scripts.UpgradeFrame.BallUpdaters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameSystem
{
    public class MatchManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int _maxWinRankAmount = 40;
        [SerializeField] private int _minWinRankAmount = 15;
        [SerializeField] private int _maxLoseRankAmount = 10;
        [SerializeField] private int _minLoseRankAmount = 3;
        
        [SerializeField] private float _minInactiveInterval = 1f;
        [SerializeField] private float _maxInactiveInterval = 3f;

        [Header("References")]
        [SerializeField] private List<Squad> _squads;
        [SerializeField] private Transform _ballPosition;
        [SerializeField] private BallUpgraderFabric _ballUpgraderFabric;
        [SerializeField] private List<Frame> _frames;

        private SquadDeathHandler _deathHandler;
        private BallUpgradeHolder _ballUpgradeHolder;
        private FrameActivator _frameActivator;

        public event Action<int> GameOver;
        
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
            _deathHandler.Initialize(HandleGameOver);
            _frameActivator.Initialize(_ballUpgradeHolder.Upgraders);
            _frameActivator.Activate();
        }

        private void HandleGameOver(bool isWin)
        {
            int rankAmount = 
                isWin ? 
                Random.Range(_minWinRankAmount, _maxWinRankAmount) : 
                Random.Range(_minLoseRankAmount, _maxLoseRankAmount);
            
            if (isWin)
            {
                EffectID.Confetti.PlayEffect(transform);
            }
            
            GameOver?.Invoke(rankAmount);
        }
    }
}