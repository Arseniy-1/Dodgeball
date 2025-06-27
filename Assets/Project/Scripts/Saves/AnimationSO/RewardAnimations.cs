using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardAnimations", menuName = "Data/RewardAnimations")]
public class RewardAnimations : ScriptableObject
{
    [field: SerializeField] public DodgeAnimationData[] DodgeAnimations { get; private set; }
    [field: SerializeField] public CelebrateAnimationData[] CelebrateAnimations { get; private set; }
    [field: SerializeField] public DeathAnimationData[] DeathAnimations { get; private set; }
    [field: SerializeField] public PrepareAnimationData[] PrepareAnimations { get; private set; }

    [Serializable]
    public struct DodgeAnimationData
    {
        [HideLabel, HorizontalGroup]
        public Constans.DodgeAnimations AnimationType;
        [HideLabel, HorizontalGroup]
        public string Name;
    }

    [Serializable]
    public struct CelebrateAnimationData
    {
        [HideLabel, HorizontalGroup]
        public Constans.CelebrateAnimations AnimationType;
        [HideLabel, HorizontalGroup]
        public string Name;
    }

    [Serializable]
    public struct DeathAnimationData
    {
        [HideLabel, HorizontalGroup]
        public Constans.DeathAnimations AnimationType;
        [HideLabel, HorizontalGroup]
        public string Name;
    }

    [Serializable]
    public struct PrepareAnimationData
    {
        [HideLabel, HorizontalGroup]
        public Constans.PrepareAnimations AnimationType;
        [HideLabel, HorizontalGroup]
        public string Name;
    }
}