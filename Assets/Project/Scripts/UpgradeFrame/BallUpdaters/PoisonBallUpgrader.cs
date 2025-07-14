using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class PoisonBallUpgrader : BallUpgrader
{
    private const int PoisonIterationCount = 5;
    private const float PoisonIterationDelay = 0.3f;
    private const int MinPoisonIterationDamage = 1;
    private const int MaxPoisonIterationDamage = 5;
    
    private const float ExplosionRadius = 2;

    private CompositeDisposable _compositeDisposable;

    public PoisonBallUpgrader(BallUpgradeInfo ballUpgradeInfo) : base(ballUpgradeInfo)
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

        Collider[] hitColliders = Physics.OverlapSphere(collision.transform.position, ExplosionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Health health))
            {
                TakePoisonDamage(health).Forget();
            }
        }
        
        _compositeDisposable.Dispose();
    }

    private async UniTask TakePoisonDamage(Health health)
    {
        for (int i = 0; i < PoisonIterationCount; i++)
        {
            int damage = Random.Range(MinPoisonIterationDamage, MaxPoisonIterationDamage);
            health.TakeDamage(damage);

            await UniTask.Delay(TimeSpan.FromSeconds(PoisonIterationDelay));
        }
    }
}