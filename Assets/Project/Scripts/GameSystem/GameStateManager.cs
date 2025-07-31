using Project.Scripts.Messages;
using UniRx;
using UnityEngine;

namespace Project.Scripts.GameSystem
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private MapFactory _mapFactory;
        [SerializeField] private UIHandler _uiHandler;

        private CompositeDisposable _disposable = new();

        private void OnEnable()
        {
            MessageBrokerHolder.GameActions
                .Receive<M_GameOver>()
                .Subscribe(_ => OnGameOver())
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }

        private void OnGameOver()
        {
            _mapFactory.ClearEntities();
        }
    }
}