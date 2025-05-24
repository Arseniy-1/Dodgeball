using UnityEngine;

public struct M_BallTaken
{
    public M_BallTaken(Entity entity)
    {
        Entity = entity;
    }
    
    public Entity Entity { get; private set; }
}