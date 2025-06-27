using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CelebrateAnimations", menuName = "Data/RewardAnimations/Celebrate")]
public class CelebrateRewardAnimations : ScriptableObject
{
    [field: SerializeField] public AnimationData[] RewardAnimations { get; private set; }
    
    [Serializable]
    public struct AnimationData
    {
        [HideLabel]
        [HorizontalGroup]
        public Constans.CelebrateAnimations CelebrateAnimation;

        [HideLabel]
        [HorizontalGroup]
        public string Name;
    }
}