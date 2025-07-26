using System;
using Project.Scripts.Services;
using Sirenix.OdinInspector;

namespace Project.Scripts.Saves.AnimationSO
{
    [Serializable]
    public struct DodgeAnimationData
    {
        [HideLabel]
        [HorizontalGroup]
        public DodgeAnimations AnimationType;
        
        [HideLabel] 
        [HorizontalGroup]
        public string Name;
    }
}