using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

namespace Project.Scripts.Services
{
    public class AnimatorController
    {
        private readonly Animator _animator;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly int _idle = Animator.StringToHash(Constants.ConstantAnimations.Idle.ToString());
        private readonly int _run = Animator.StringToHash(Constants.ConstantAnimations.Run.ToString());

        private readonly int _throw = Animator.StringToHash(Constants.ConstantAnimations.Throw.ToString());
        private readonly int _prepareToThrow = Animator.StringToHash(Constants.ConstantAnimations.PrepareToThrow.ToString());

        private readonly int _dodgeIdle = Animator.StringToHash(Constants.ConstantAnimations.DodgeIdle.ToString());

        public AnimatorController(Animator animator)
        {
            _animator = animator;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        public UniTask Attack() => PlayAndWait(_throw, _cancellationTokenSource.Token);

        public void PrepareAttack()
        {
            if (_animator != null)
                _animator.Play(_prepareToThrow);
        }

        public void Run()
        {
            if (_animator != null)
                _animator.Play(_run);
        }

        public void Idle()
        {
            if (_animator != null)
                _animator.Play(_idle);
        }

        public void DodgeIdle()
        {
            if (_animator != null)
                _animator.Play(_dodgeIdle);
        }

        public void Celebrate()
        {
            var animationHashes = YG2.saves.AnimationsHolder.CelebrateAnimationsHash;
            int randomCelebrate = animationHashes[Random.Range(0, animationHashes.Count)];

            _animator.Play(randomCelebrate);
        }

        public void PrepareToBattle()
        {
            var animationHashes = YG2.saves.AnimationsHolder.PrepareAnimationsHash;
            int randomPrepare = animationHashes[Random.Range(0, animationHashes.Count)];

            _animator.Play(randomPrepare);
        }

        public UniTask Death()
        {
            var animationHashes = YG2.saves.AnimationsHolder.DeathAnimationsHash;
            int randomDeath = animationHashes[Random.Range(0, animationHashes.Count)];

            return PlayAndWait(randomDeath, _cancellationTokenSource.Token);
        }

        public UniTask Dodge()
        {
            var animationHashes = YG2.saves.AnimationsHolder.DodgeAnimationsHash;
            int randomDodge = animationHashes[Random.Range(0, animationHashes.Count)];

            return PlayAndWait(randomDodge, _cancellationTokenSource.Token);
        }

        private async UniTask PlayAndWait(int animationHash, CancellationToken token)
        {
            _animator.Play(animationHash);
            await UniTask.Yield();

            if (token.IsCancellationRequested || _animator == null)
                return;

            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            int attempts = 0;

            while (token.IsCancellationRequested == false && stateInfo.shortNameHash != animationHash && attempts < 60)
            {
                await UniTask.Yield();

                if (token.IsCancellationRequested || _animator == null)
                    return;

                stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                attempts++;
            }

            while (token.IsCancellationRequested == false && stateInfo.normalizedTime < 1f && !_animator.IsInTransition(0))
            {
                await UniTask.Yield();

                if (token.IsCancellationRequested || _animator == null)
                    return;

                stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            }
        }
    }
}