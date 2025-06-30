using System.Threading;
using TMPro;
using UnityEngine;

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
        _rewardName.text = rewardName;
        _rewardEffect.Play();
    }

    public void Disable()
    {
        _rewardName.text = "";
        _rewardEffect.Stop();
        gameObject.SetActive(false);
        _cancellationTokenSource.Cancel();
    }
}