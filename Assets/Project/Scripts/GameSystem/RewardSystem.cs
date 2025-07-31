using Cysharp.Threading.Tasks;
using Project.Scripts.Messages;
using Project.Scripts.Rank;
using Project.Scripts.Reward;
using UniRx;
using UnityEngine;

namespace Project.Scripts.GameSystem
{
    public class RewardSystem
    {
        private readonly RankHolder _rankHolder;
        private readonly RewardService _rewardService;
        private readonly UIHandler _uiHandler;
        
        private int _maxWinRankAmount = 40;
        private int _minWinRankAmount = 15;
        private int _maxLoseRankAmount = 10;
        private int _minLoseRankAmount = 3;
        
        private bool _rewardRaised = false;
        private CompositeDisposable _disposable;

        public RewardSystem(RankHolder rankHolder, RewardService rewardService, UIHandler uiHandler)
        {
            _rankHolder = rankHolder;
            _rewardService = rewardService;
            _uiHandler = uiHandler;
        }

        public void Initialize()
        {
            _disposable = new CompositeDisposable();

            MessageBrokerHolder.GameActions
                .Receive<M_GameOver>()
                .Subscribe(message => HandleGameOver(message.IsPlayerWin).Forget())
                .AddTo(_disposable);
            
            _rankHolder.RankRaised += HandleRankRaised;
            _uiHandler.RankCanvasClosed += HandleRankCanvasClose;
        }

        public void Dispose()
        {
            _disposable.Dispose();
            
            _rankHolder.RankRaised -= HandleRankRaised;
            _uiHandler.RankCanvasClosed -= HandleRankCanvasClose;
        }

        private async UniTaskVoid HandleGameOver(bool isPlayerWin)
        {
            int rankAmount = isPlayerWin
                ? Random.Range(_minWinRankAmount, _maxWinRankAmount) 
                : Random.Range(_minLoseRankAmount, _maxLoseRankAmount);
            
            await _uiHandler.GameOver();

            if (_rewardRaised)
            {
                _uiHandler.GiveReward();
            } 
            
            _rankHolder.IncreaseRank(rankAmount);
        }

        private void HandleRankCanvasClose()
        {
            _rewardRaised = false;
        }
        
        private void HandleRankRaised()
        {
            _rewardRaised = true;
        }
    }
}