using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected BallThrower BallThrower;
    [SerializeField] protected BallHolder BallHolder;
    [SerializeField] protected CollisionHandler CollisionHandler;
    [SerializeField] protected TargetScanner TargetScanner;
    [SerializeField] protected Mover Mover;
    [SerializeField] protected Health Health;
    [SerializeField] protected List<Entity> Teammates;
    [SerializeField] protected Animator Animator;
    [SerializeField] protected HitCheker HitCheker;
    
    [SerializeField] protected HealthCanvas HealthCanvas;
    
    public string CurrentState;
    public List<string> CurrentStates = new List<string>();

    protected TargetProvider TargetProvider = new TargetProvider();
    protected Collider SquadZone;
    protected Collider Collider;
    protected Rigidbody Rigidbody;
    protected AnimatorController AnimatorController;

    protected StateMaсhine StateMachine;

    [SerializeField] protected Ball Ball;
    
    private void OnEnable()
    {
        Health.LostHealth += HandleLostHealth;
    }

    private void OnDisable()
    {
        Health.LostHealth -= HandleLostHealth;
    }

    protected virtual void Update()
    {
        StateMachine.Update();

        CurrentState = StateMachine._currentState.ToString();

        CurrentStates.Clear();
        
        foreach (var state in StateMachine._states.Values)
            CurrentStates.Add(state.ToString());
    }

    public virtual void Initialize(Collider squadZone, List<Entity> teammates, Ball ball)
    {
        Collider = GetComponent<Collider>();
        Rigidbody = GetComponent<Rigidbody>();
        Teammates = teammates;
        SquadZone = squadZone;
        Health.Initialize(CollisionHandler);
        Ball = ball;

        if (Animator != null)
            AnimatorController = new AnimatorController(Animator);
    }

    public virtual void Reset()
    {
        CollisionHandler.enabled = true;
        Collider.enabled = true;
        BallHolder.LostBall();
        HealthCanvas.gameObject.SetActive(true);
    }
    
    public abstract void Celebrate();
    
    [Button]
    public virtual void Die()
    {
        StateMachine.Dispose();
        AnimatorController.Dispose();
        Health.Reset();
    }   
    
    protected abstract void HandleLostHealth();
    
    protected async UniTask HideEntity()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Vector3 start = transform.position;
        Vector3 target = start + Vector3.down * 2f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(start, target, t);
            await UniTask.Yield();
        }

        transform.position = target;
    }
}