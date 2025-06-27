using UnityEngine;

public class RewardModel : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ModelRotator _rotator;
    
    public void PlayAnimation(int animationHash)
    {
        _animator.Play(animationHash);
    }

    private void Update()
    {
        _rotator.Update(transform);
    }
}