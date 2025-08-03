using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Messages;
using Project.Scripts.Rank;
using Project.Scripts.Reward;
using Project.Scripts.UI;
using Project.Scripts.UI.Canvases;
using Project.Scripts.UI.View;
using UnityEngine;
using YG;

namespace Project.Scripts.GameSystem
{
    [Serializable]
    public class UIHandler
    {
        [SerializeField] private StartGameCanvas _startGameCanvas;
        [SerializeField] private RankViewCanvas _rankViewCanvas;
        [SerializeField] private RewardCanvas _rewardCanvas;
        [SerializeField] private GameUICanvas _gameCanvas;
        [SerializeField] private TutorialCanvas _tutorialCanvas;
        [SerializeField] private UserInfoView _userInfoView;
        [SerializeField] private RewardButton _rewardButton;

        public event Action StartButtonPressed;
        public event Action RankCanvasClosed;

        public void Initialize(RewardService rewardService, RankHolder rankHolder)
        {
            _rewardCanvas.Initialize(rewardService);

            _rankViewCanvas.Initialize(rankHolder);

            _userInfoView.Initialize(rankHolder);
        }

        public void Enable()
        {
            _startGameCanvas.StartGameButtonPressed += OnStartGameButtonPressed;
            _rankViewCanvas.RewardViewClosed += OnRewardViewClosed;
            _rewardButton.ButtonClicked += OnRewardButtonClicked;
        }

        public void Disable()
        {
            _startGameCanvas.StartGameButtonPressed -= OnStartGameButtonPressed;
            _rankViewCanvas.RewardViewClosed -= OnRewardViewClosed;
            _rewardButton.ButtonClicked -= OnRewardButtonClicked;
        }

        public void Start()
        {
            _startGameCanvas.gameObject.SetActive(true);
        }

        public void GiveReward()
        {
            _startGameCanvas.gameObject.SetActive(false);
            _rewardCanvas.gameObject.SetActive(true);

            _rewardCanvas.RewardCanvasClosed += OnRewardCanvasClosed;
        }
        
        public async UniTask GameOver()
        {
            _gameCanvas.gameObject.SetActive(false);
            float waitTime = 3f;
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime));

            _rankViewCanvas.gameObject.SetActive(true);
            await _rankViewCanvas.ShowResultsAsync();
            _rankViewCanvas.gameObject.SetActive(false);
        }
        
        private void OnStartGameButtonPressed()
        {
            _tutorialCanvas.gameObject.SetActive(false);

            if (YG2.saves.ProgressData.IsFirstSession)
            {
                YG2.saves.ProgressData.IsFirstSession = false;
                YG2.SaveProgress();

                _tutorialCanvas.gameObject.SetActive(true);
            }

            _startGameCanvas.gameObject.SetActive(false);
            _gameCanvas.gameObject.SetActive(true);

            MessageBrokerHolder.GameActions.Publish(new M_GameStarted());

            StartButtonPressed?.Invoke();
        }

        private void OnRewardButtonClicked()
        {
            string id = "coin";

            YG2.RewardedAdvShow(id, GiveReward);
        }

        private void OnRewardCanvasClosed()
        {
            _rewardCanvas.RewardCanvasClosed -= OnRewardCanvasClosed;
            
            _rewardCanvas.gameObject.SetActive(false);
            _startGameCanvas.gameObject.SetActive(true);
        }

        private void OnRewardViewClosed()
        {
            _rankViewCanvas.gameObject.SetActive(false);
            _startGameCanvas.gameObject.SetActive(true);
            
            RankCanvasClosed?.Invoke();
        }
    }
}