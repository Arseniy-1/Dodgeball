using System;
using UniRx;

[Serializable]
public class EffectsSpawner : Spawner<Effect>
{
    protected CompositeDisposable CompositeDisposable;
    
    public EffectsSpawner(Effect effect)
    {
        Prefab = effect;
        Pool = new EffectsPool(Prefab, StartAmount);
        
        CompositeDisposable = new CompositeDisposable();
    }
    
    public void Dispose()
    {
        CompositeDisposable.Dispose();
    }
}