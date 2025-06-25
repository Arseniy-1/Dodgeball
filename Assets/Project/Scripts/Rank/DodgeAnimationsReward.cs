using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Rewards", menuName = "RewardSystem/EffectRewards", order = 51)]
public class DodgeAnimationsReward : ScriptableObject
{
    [SerializeField] private RewardData[] _rewards;
    
    public struct RewardData
    {
        [HideLabel]
        [HorizontalGroup]
        public AnimationDodgeRewardID AnimationDodgeRewardID;
        [HideLabel]
        [HorizontalGroup]
        public Constans.Animations DodgeAnimationID;
    }
}