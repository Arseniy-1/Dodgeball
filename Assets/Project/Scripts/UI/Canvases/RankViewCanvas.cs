using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Scripts.Rank;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.Canvases
{
    public class RankViewCanvas : InteractiveCanvas
    {
        private const int MaxAmount = 100;
        
        [SerializeField] private Image _chestFillImage;
        [SerializeField] private TMP_Text _percentageText;

        private RankHolder _rankHolder;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action RewardViewClosed;
    
        public void Initialize(RankHolder rankHolder)
        {
            _rankHolder = rankHolder;
        }

        public UniTask ShowResultsAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            return AnimateProgressAsync(_rankHolder.PreviousAmount, _rankHolder.CurrentAmount, _cancellationTokenSource.Token);
        }
        
        protected override void HandleButtonClick()
        {
            RewardViewClosed?.Invoke();
            _cancellationTokenSource.Cancel();
        }

        private async UniTask AnimateProgressAsync(int from, int to, CancellationToken cancellationToken)
        {
            float duration = 2.5f;
            float time = 0f;
            float maxPercent = 100f;

            float startFill = from / (float)MaxAmount;
            float endFill = to / (float)MaxAmount;

            while (cancellationToken.IsCancellationRequested == false || time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;

                float currentFill = Mathf.Lerp(startFill, endFill, t);
                _chestFillImage.fillAmount = currentFill;

                int percent = Mathf.RoundToInt(currentFill * maxPercent);
                _percentageText.text = percent + "%";

                await UniTask.Yield();
            
                if (cancellationToken.IsCancellationRequested)
                    return;
            }

            _chestFillImage.fillAmount = endFill;
            _percentageText.text = (to * maxPercent / MaxAmount) + "%";
        }
    }
}