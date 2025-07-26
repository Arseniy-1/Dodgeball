using System;
using Cysharp.Threading.Tasks;
using Project.Scripts.Services.EffectService;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.UpgradeFrame.BallUpdaters
{
    public class PoisonBallUpgrade : BallUpgrade
    {
        private const int PoisonIterationCount = 5;
        private const float PoisonIterationDelay = 0.3f;
        private const int MinPoisonIterationDamage = 1;
        private const int MaxPoisonIterationDamage = 5;
    
        private const float ExplosionRadius = 2;

        private Collider[] _hitCollidersBuffer = new Collider[20];
        
        private CompositeDisposable _compositeDisposable;

        public PoisonBallUpgrade(BallUpgradeInfo ballUpgradeInfo) 
            : base(ballUpgradeInfo)
        {
        }

        public override void UpgradeBall(Ball ball)
        {
            _compositeDisposable = new CompositeDisposable();

            ball.OnCollisionEnterAsObservable()
                .Subscribe(collision =>
                {
                    HandleHit(collision, ball.transform);
                })
                .AddTo(_compositeDisposable);
        }

        private void HandleHit(Collision collision, Transform ballTransform)
        {
            EffectID.PoisonExplosion.PlayEffect(ballTransform);

            int hitsCount = Physics.OverlapSphereNonAlloc(
                collision.transform.position, 
                ExplosionRadius, 
                _hitCollidersBuffer
            );

            for (int i = 0; i < hitsCount; i++)
            {
                if (_hitCollidersBuffer[i].TryGetComponent(out HealthSystem.Health health))
                {
                    TakePoisonDamage(health).Forget();
                }
            }

            _compositeDisposable.Dispose();
        }

        private async UniTask TakePoisonDamage(HealthSystem.Health health)
        {
            for (int i = 0; i < PoisonIterationCount; i++)
            {
                int damage = Random.Range(MinPoisonIterationDamage, MaxPoisonIterationDamage);
                health.TakeDamage(damage);

                await UniTask.Delay(TimeSpan.FromSeconds(PoisonIterationDelay));
            }
        }
    }
}