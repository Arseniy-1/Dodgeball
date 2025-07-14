using System.Threading;
using TMPro;
using UnityEngine;
using YG;

public class GameUICanvas : GameCanvas
{
    [SerializeField] private TextMeshProUGUI _enemyName;
    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private TextMeshProUGUI _timeView;
    
    private Timer _timer;
    private CancellationTokenSource _cancellationTokenSource;

    private void Awake()
    {
        _timer = new Timer(_timeView);
    }

    private void OnEnable()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        
        _enemyName.text = Constans.EnemyNames.GetRandomName();
        _playerName.text = YG2.player.name;
        
        _timer.Start(_cancellationTokenSource.Token).Forget();
    }

    private void OnDisable()
    {
        _cancellationTokenSource?.Cancel();
        _timer.Reset();
    }
}