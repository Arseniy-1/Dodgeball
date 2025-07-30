using System;
using Project.Scripts.Services;
using Sirenix.OdinInspector;

namespace Project.Scripts.SavesSystem.AnimationSO
{
    [Serializable]
    public struct CelebrateAnimationData
    {
        [HideLabel] 
        [HorizontalGroup]
        public CelebrateAnimations AnimationType;
        
        [HideLabel]
        [HorizontalGroup]
        public string Name;
    }
}