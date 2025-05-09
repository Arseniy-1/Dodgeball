public class TargetProvider
{
    public Entity Target { get; private set; }
    
    public void SelectTarget(Entity target)
    {
        Target = target;
    }
    
    public void DeselectTarget()
    {
        
    }
}