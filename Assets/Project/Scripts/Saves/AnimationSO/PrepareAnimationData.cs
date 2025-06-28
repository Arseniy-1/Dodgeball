using System;
using Sirenix.OdinInspector;

[Serializable]
public struct PrepareAnimationData
{
    [HideLabel, HorizontalGroup]
    public Constans.PrepareAnimations AnimationType;
    [HideLabel, HorizontalGroup]
    public string Name;
}