using System;
using UniRx;
using UnityEngine;

[Serializable]
public class EffectsSpawner : Spawner<Effect>
{
    protected CompositeDisposable CompositeDisposable;

    public EffectsSpawner(Effect effect, Transform parent)
    {
        Prefab = effect;
        Pool = new EffectsPool(Prefab, StartAmount, parent);

        CompositeDisposable = new CompositeDisposable();
    }

    public void Dispose()
    {
        CompositeDisposable.Dispose();
    }
}