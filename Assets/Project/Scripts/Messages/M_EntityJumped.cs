using UnityEngine;

public struct M_EntityJumped
{
    public M_EntityJumped(Transform entityTransform)
    {
        EntityTransform = entityTransform;
    }
    
    public Transform EntityTransform { get; private set; }
}