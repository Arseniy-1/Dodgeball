using UnityEngine;
using System.Collections.Generic;

public class AnimatorController
{
    private Animator _animator;

    private int _run = Animator.StringToHash(Constans.Animation.Run);
    private int _throw = Animator.StringToHash(Constans.Animation.Attack);
    private int _prepareToThrow = Animator.StringToHash(Constans.Animation.PrepareAttack);
    private int _dodgeIdle = Animator.StringToHash(Constans.Animation.DodgeIdle);

    private List<int> _dodges = new List<int>
    {
        Animator.StringToHash(Constans.Animation.DodgeRight),
        Animator.StringToHash(Constans.Animation.DodgeLeft),
        Animator.StringToHash(Constans.Animation.DodgeBackflip),
    };

    public AnimatorController(Animator animator) => _animator = animator;

    public void Run() => _animator.Play(_run);

    public void Attack()
    {
        _animator.Play(_throw);
        _animator.
    }

    public void PrepareAttack() => _animator.Play(_prepareToThrow);
    public void DodgeIdle() => _animator.Play(_dodgeIdle);

    public void Dodge()
    {
        int randomDodge = _dodges[Random.Range(0, _dodges.Count)];

        _animator.Play(randomDodge);
    }
}