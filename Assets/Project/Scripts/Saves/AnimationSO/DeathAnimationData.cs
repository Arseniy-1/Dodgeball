using System;
using Sirenix.OdinInspector;

[Serializable]
public struct DeathAnimationData
{
    [HideLabel, HorizontalGroup]
    public Constans.DeathAnimations AnimationType;
    [HideLabel, HorizontalGroup]
    public string Name;
}