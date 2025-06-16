using UnityEngine;

public struct M_EntityHited
{
    public M_EntityHited(Transform entityTransform)
    {
        EntityTransform = entityTransform;
    }
    
    public Transform EntityTransform { get; private set; }
}