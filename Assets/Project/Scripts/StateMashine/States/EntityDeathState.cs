public abstract class EntityDeathState : IState
{
    private readonly AnimatorController _animatorController;

    protected EntityDeathState(AnimatorController animatorController)
    {
        _animatorController = animatorController;
    }

    public virtual void Enter()
    {
        _animatorController.Death();
    }

    public virtual void Exit() { }
    
    public void Initialize(IStateSwitcher stateSwitcher)
    {
    }

    public virtual void Update() { }
}