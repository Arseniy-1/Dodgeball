using System;
using Sirenix.OdinInspector;

[Serializable]
public struct DodgeAnimationData
{
    [HideLabel, HorizontalGroup]
    public Constans.DodgeAnimations AnimationType;
    [HideLabel, HorizontalGroup]
    public string Name;
}