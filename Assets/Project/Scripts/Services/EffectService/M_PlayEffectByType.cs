using UnityEngine;

public struct M_PlayEffectByType
{
    public EffectID EffectID { get; private set; }
    public Transform Transform { get; private set; }

    public M_PlayEffectByType(EffectID effectID, Transform transform)
    {
        EffectID = effectID;
        Transform = transform;
    }
}