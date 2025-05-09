using UnityEngine;

public struct M_BallChangedZone
{
    public M_BallChangedZone(Collider collider)
    {
        Zone = collider;
    }
    
    public Collider Zone { get; private set; }
}