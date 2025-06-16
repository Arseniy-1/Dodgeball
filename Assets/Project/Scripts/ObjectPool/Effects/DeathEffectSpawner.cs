using System;
using UniRx;

[Serializable]
public class DeathEffectsSpawner : Spawner<Effect>
{
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    
    public DeathEffectsSpawner(Effect effect)
    {
        Prefab = effect;
        Pool = new DeathEffectsPool(Prefab, StartAmount);
        
        MessageBrokerHolder.GameActions.Receive<M_EntityDeath>().Subscribe((message) => HandleEnemyDeath(message.Entity))
            .AddTo(_compositeDisposable);
    }

    private void HandleEnemyDeath(Entity entity)
    {
        var effect = Spawn();
        effect.transform.position = entity.transform.position;
    }
}