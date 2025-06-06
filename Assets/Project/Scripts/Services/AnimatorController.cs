using UnityEngine;

public class AnimatorController
{
    private Animator _animator;

    private int _move = Animator.StringToHash(Constans.Animation.Run);
    private int _walk = Animator.StringToHash(Constans.Animation.Walk);
    private int _idle = Animator.StringToHash(Constans.Animation.Idle);
    private int _dying = Animator.StringToHash(Constans.Animation.Die);
    private int _attack = Animator.StringToHash(Constans.Animation.Attack);
    private int _preAttack = Animator.StringToHash(Constans.Animation.PreAttack);

    public AnimatorController(Animator animator) => _animator = animator;


    public void Die() => _animator.Play(_dying);
    public void Attack() => _animator.Play(_attack);
    public void PreAttack() => _animator.Play(_preAttack);
    public void Running() => _animator.Play(_move);
    public void Walking() => _animator.Play(_walk);
    public void Idle() => _animator.Play(_idle);

    public void Dodge()
    {
        
    }
}