using System;
using Project.Scripts.Services;
using Sirenix.OdinInspector;

namespace Project.Scripts.SavesSystem.AnimationSO
{
    [Serializable]
    public struct DeathAnimationData
    {
        [HideLabel, HorizontalGroup]
        public DeathAnimations AnimationType;
        [HideLabel, HorizontalGroup]
        public string Name;
    }
}