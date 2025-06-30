using TMPro;
using UniRx;
using UnityEngine;

public class GameUICanvas : GameCanvas
{
    [SerializeField] private TextMeshProUGUI _enemyScore;
    [SerializeField] private TextMeshProUGUI _playerScore;

    private CompositeDisposable _compositeDisposable;

    private int _enemyCount;
    private int _playerCount;

    private void OnDisable()
    {
        _compositeDisposable.Dispose();
    }

    public void Initialize(int enemyCount, int playerCount)
    {
        _enemyScore.text = enemyCount.ToString();
        _playerScore.text = playerCount.ToString();

        _enemyCount = enemyCount;
        _playerCount = playerCount;

        _compositeDisposable = new CompositeDisposable();

        MessageBrokerHolder.GameActions
            .Receive<M_EntityDeath>()
            .Subscribe((message) =>
                UpdateScore(message.Entity))
            .AddTo(_compositeDisposable);
    }

    private void UpdateScore(Entity entity)
    {
        if (entity is Enemy)
        {
            _enemyCount--;
            _enemyScore.text = _enemyCount.ToString();
        }
        else if (entity is Player)
        {
            _playerCount--;
            _playerScore.text = _playerCount.ToString();
        }
    }
}