public struct M_EntityDeath
{
    public M_EntityDeath(Entity entity)
    {
        Entity = entity;
    }
    
    public Entity Entity { get; private set; }
}