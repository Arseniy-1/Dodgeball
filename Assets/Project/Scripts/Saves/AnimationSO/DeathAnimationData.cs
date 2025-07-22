using System;
using Project.Scripts.Services;
using Sirenix.OdinInspector;

namespace Project.Scripts.Saves.AnimationSO
{
    [Serializable]
    public struct DeathAnimationData
    {
        [HideLabel, HorizontalGroup]
        public Constants.DeathAnimations AnimationType;
        [HideLabel, HorizontalGroup]
        public string Name;
    }
}