using System;
using Project.Scripts.Services;
using Sirenix.OdinInspector;

namespace Project.Scripts.Saves.AnimationSO
{
    [Serializable]
    public struct PrepareAnimationData
    {
        [HideLabel]
        [HorizontalGroup]
        public Constants.PrepareAnimations AnimationType;
        
        [HideLabel] 
        [HorizontalGroup]
        public string Name;
    }
}