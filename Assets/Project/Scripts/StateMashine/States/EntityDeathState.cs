public abstract class EntityDeathState : IState
{
    private readonly AnimatorController _animatorController;
    private readonly CollisionHandler _collisionHandler;

    protected EntityDeathState(AnimatorController animatorController, CollisionHandler collisionHandler)
    {
        _animatorController = animatorController;
        _collisionHandler = collisionHandler;
    }

    public virtual void Enter()
    {
        _animatorController.Death();
        _collisionHandler.enabled = false;
    }

    public virtual void Exit()
    {
        _collisionHandler.enabled = true;
    }
    
    public void Initialize(IStateSwitcher stateSwitcher)
    {
    }

    public virtual void Update() { }
}