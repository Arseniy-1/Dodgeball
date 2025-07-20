using System;
using Project.Scripts.Services;
using Sirenix.OdinInspector;

namespace Project.Scripts.Saves.AnimationSO
{
    [Serializable]
    public struct CelebrateAnimationData
    {
        [HideLabel, HorizontalGroup]
        public Constans.CelebrateAnimations AnimationType;
        [HideLabel, HorizontalGroup]
        public string Name;
    }
}