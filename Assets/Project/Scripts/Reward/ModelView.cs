using TMPro;
using UnityEngine;

public class ModelView : MonoBehaviour
{
    [SerializeField] private RewardModel _rewardModel;
    [SerializeField] private TextMeshProUGUI _rewardName;
    
    public void ShowReward(int animationHash, string rewardName)
    {
        _rewardModel.PlayAnimation(animationHash);
        _rewardName.text = rewardName;
    }
}