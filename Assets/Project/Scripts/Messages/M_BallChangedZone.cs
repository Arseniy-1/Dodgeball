using UnityEngine;

public struct M_BallChangedZone
{
    public M_BallChangedZone(Vector3 position)
    {
        Position = position;
    }
    
    public Vector3 Position { get; private set; }
}