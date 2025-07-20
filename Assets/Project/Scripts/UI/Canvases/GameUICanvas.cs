using System.Threading;
using Project.Scripts.Services;
using Project.Scripts.Settings;
using TMPro;
using UnityEngine;
using YG;
using YG.LanguageLegacy;

namespace Project.Scripts.UI.Canvases
{
    public class GameUICanvas : GameCanvas
    {
        [SerializeField] private TextMeshProUGUI _enemyName;
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private TextMeshProUGUI _timeView;

        [SerializeField] private LanguageYG _languageTranslator;

        private Timer _timer;
        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            _timer = new Timer(_timeView);
        }

        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            switch (YG2.lang)
            {
                case nameof(Languages.en):
                    _enemyName.text = Constans.EnemyNames.GetRandomEnglishName();
                    break;
            
                case nameof(Languages.ru):
                    _enemyName.text = Constans.EnemyNames.GetRandomRussianName();
                    break;
            
                case nameof(Languages.tr):
                    _enemyName.text = Constans.EnemyNames.GetRandomTurkishName();
                    break;
            }

            if (YG2.player.auth)
            {
                _languageTranslator.enabled = false;
                _playerName.text = YG2.player.name;
            }

            _timer.Start(_cancellationTokenSource.Token).Forget();
        }

        private void OnDisable()
        {
            _cancellationTokenSource?.Cancel();
            _timer.Reset();
        }
    }
}