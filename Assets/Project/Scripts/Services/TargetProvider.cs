using Project.Scripts.Entities;

namespace Project.Scripts.Services
{
    public class TargetProvider
    {
        public Entity Target { get; private set; }
    
        public void SelectTarget(Entity target)
        {
            Target = target;
        }
    }
}