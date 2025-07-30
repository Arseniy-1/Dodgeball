using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;

namespace Project.Scripts.UI
{
    public class Timer
    {
        private readonly TextMeshProUGUI _timeText;
    
        private int _totalSeconds;

        public Timer(TextMeshProUGUI timeText)
        {
            _timeText = timeText;
        }

        public async UniTaskVoid Start(CancellationToken token)
        {
            int delay = 1;
            
            while (token.IsCancellationRequested == false)
            {
                _totalSeconds++;
                UpdateTimeDisplay();
            
                await UniTask.Delay(TimeSpan.FromSeconds(delay), DelayType.DeltaTime, PlayerLoopTiming.Update, token);
            }
        }

        public void Reset()
        {
            _totalSeconds = 0;
            UpdateTimeDisplay();
        }

        private void UpdateTimeDisplay()
        {
            int secondsPerMinute = 60;
            
            int minutes = _totalSeconds / secondsPerMinute;
            int seconds = _totalSeconds % secondsPerMinute;
        
            _timeText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}