using System;
using Sirenix.OdinInspector;

[Serializable]
public struct CelebrateAnimationData
{
    [HideLabel, HorizontalGroup]
    public Constans.CelebrateAnimations AnimationType;
    [HideLabel, HorizontalGroup]
    public string Name;
}