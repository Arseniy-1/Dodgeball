using System;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class PoisonBallUpgrader : BallUpgrader
{
    private const int PoisonIterationCount = 12;
    private const float PoisonIterationDelay = 0.7f;
    private const int MinPoisonIterationDamage = 1;
    private const int MaxPoisonIterationDamage = 5;

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
                HandleHit(collision);
            })
            .AddTo(_compositeDisposable);
    }

    private async void HandleHit(Collision collision)
    {
        _compositeDisposable.Dispose();

        if (collision.collider.TryGetComponent(out Health health))
        {
            for (int i = 0; i < PoisonIterationCount; i++)
            {
                if(health.CurrentHealthPoint == 0)
                    return;
                
                int damage = Random.Range(MinPoisonIterationDamage, MaxPoisonIterationDamage);
                health.TakeDamage(damage);
                EffectID.PoisonExplosion.PlayEffect(health.transform);

                await UniTask.Delay(TimeSpan.FromSeconds(PoisonIterationDelay));
            }
        }
    }
}