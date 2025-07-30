using System;
using Project.Scripts.Services;
using Sirenix.OdinInspector;

namespace Project.Scripts.SavesSystem.AnimationSO
{
    [Serializable]
    public struct PrepareAnimationData
    {
        [HideLabel]
        [HorizontalGroup]
        public PrepareAnimations AnimationType;
        
        [HideLabel] 
        [HorizontalGroup]
        public string Name;
    }
}