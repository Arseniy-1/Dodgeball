using UnityEngine;

public struct M_EntityDodged
{
    public M_EntityDodged(Transform entityTransform)
    {
        EntityTransform = entityTransform;
    }
    
    public Transform EntityTransform { get; private set; }
}