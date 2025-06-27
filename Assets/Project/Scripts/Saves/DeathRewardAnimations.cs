using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathRewardAnimations", menuName = "Data/RewardAnimations/Death")]
public class DeathRewardAnimations : ScriptableObject
{
    [field: SerializeField] public AnimationData[] RewardAnimations { get; private set; }
    
    [Serializable]
    public struct AnimationData
    {
        [HideLabel]
        [HorizontalGroup]
        public Constans.DeathAnimations DeathAnimation;

        [HideLabel]
        [HorizontalGroup]
        public string Name;
    }
}