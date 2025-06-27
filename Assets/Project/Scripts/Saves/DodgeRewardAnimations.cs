using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DodgeRewardAnimations", menuName = "Data/RewardAnimations/Dodge")]
public class DodgeRewardAnimations : ScriptableObject
{
    [field: SerializeField] public AnimationData[] RewardAnimations { get; private set; }
    
    [Serializable]
    public struct AnimationData
    {
        [HideLabel] 
        [HorizontalGroup] 
        public Constans.DodgeAnimations DodgeAnimation;

        [HideLabel] 
        [HorizontalGroup] 
        public string Name;
    }
}