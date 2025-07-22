using System.Threading;
using Assets.SimpleLocalization.Scripts;
using TMPro;
using UnityEngine;

namespace Project.Scripts.Reward
{
    public class ModelView : MonoBehaviour
    {
        [SerializeField] private RewardModel _rewardModel;
        [SerializeField] private TextMeshProUGUI _rewardName;
        [SerializeField] private ParticleSystem _rewardEffect;

        private CancellationTokenSource _cancellationTokenSource;
    
        public void ShowReward(int animationHash, string rewardName)
        {
            _cancellationTokenSource = new CancellationTokenSource();
        
            _rewardModel.PlayAnimation(animationHash, _cancellationTokenSource.Token);
            _rewardName.text = LocalizationManager.Localize(rewardName);
            _rewardEffect.Play();
        }

        public void Disable()
        {
            _rewardName.text = string.Empty;
            _rewardEffect.Stop();
            gameObject.SetActive(false);
            _cancellationTokenSource.Cancel();
        }
    }
}