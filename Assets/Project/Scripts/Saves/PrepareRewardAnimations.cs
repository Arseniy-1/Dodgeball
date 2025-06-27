using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PrepareRewardAnimations", menuName = "Data/RewardAnimations/Prepare")]
public class PrepareRewardAnimations : ScriptableObject
{
    [field: SerializeField] public AnimationData[] RewardAnimations { get; private set; }
    
    [Serializable]
    public struct AnimationData
    {
        [HideLabel]
        [HorizontalGroup]
        public Constans.PrepareAnimations PrepareAnimation;

        [HideLabel]
        [HorizontalGroup]
        public string Name;
    }
}