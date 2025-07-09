using UnityEngine;

public struct M_PlayEffectByType
{
    public EffectID EffectID { get; private set; }
    public Transform Transform { get; private set; }
    public bool IsParent { get; private set; }

    public M_PlayEffectByType(EffectID effectID, Transform transform, bool isParent = false)
    {
        EffectID = effectID;
        Transform = transform;
        IsParent = isParent;
    }
}