using System;
using UniRx;
using UnityEngine;

[Serializable]
public class HitEffectsSpawner : Spawner<Effect>
{
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    
    public HitEffectsSpawner(Effect effect)
    {
        Prefab = effect;
        Pool = new DeathEffectsPool(Prefab, StartAmount);
        
        MessageBrokerHolder.GameActions.Receive<M_EntityHited>().Subscribe((message) => HandleEnemyDeath(message.EntityTransform))
            .AddTo(_compositeDisposable);
    }

    private void HandleEnemyDeath(Transform transform)
    {
        var effect = Spawn();
        effect.transform.position = transform.position;
    }
}