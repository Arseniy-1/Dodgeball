using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;

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
        while (token.IsCancellationRequested == false)
        {
            _totalSeconds++;
            UpdateTimeDisplay();
            
            await UniTask.Delay(1000, DelayType.DeltaTime, PlayerLoopTiming.Update, token);
        }
    }

    public void Reset()
    {
        _totalSeconds = 0;
        UpdateTimeDisplay();
    }

    private void UpdateTimeDisplay()
    {
        int minutes = _totalSeconds / 60;
        int seconds = _totalSeconds % 60;
        
        _timeText.text = $"{minutes:00}:{seconds:00}";
    }
}