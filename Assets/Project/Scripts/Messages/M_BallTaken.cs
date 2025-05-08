using UnityEngine;

public struct M_BallTaken
{
    public M_BallTaken(Vector3 position)
    {
        Position = position;
    }
    
    public Vector3 Position { get; private set; }
}