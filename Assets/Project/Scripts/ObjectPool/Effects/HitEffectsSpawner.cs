using System;
using UniRx;
using UnityEngine;

[Serializable]
public class HitEffectsSpawner : EffectsSpawner
{
    public HitEffectsSpawner(Effect effect) : base(effect)
    {
        MessageBrokerHolder.GameActions
            .Receive<M_EntityHited>()
            .Subscribe((message) => 
                HandleEnemyHit(message.EntityTransform))
            .AddTo(CompositeDisposable);
    }

    private void HandleEnemyHit(Transform transform)
    {
        var effect = Spawn();
        effect.transform.position = transform.position;
    }
}