using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using EffectData = EffectsSetting.EffectData;

[Serializable]
public class EffectService : IDisposable
{
    [OdinSerialize] private Dictionary<EffectID, EffectData> _effectsData;
    
    private Dictionary<EffectID, EffectsSpawner> _spawners;
    private CompositeDisposable _compositeDisposable;
    
    public EffectService(Dictionary<EffectID, EffectData> effectsData)
    {
        _effectsData = effectsData;
        
        var poolHolder = new GameObject("EffectsPoolHolder");
        Object.DontDestroyOnLoad(poolHolder);

        _spawners = new Dictionary<EffectID, EffectsSpawner>();
        
        foreach (var pair in _effectsData)
            _spawners[pair.Key] = new EffectsSpawner(pair.Value.Effect, poolHolder.transform);
        
        _compositeDisposable = new CompositeDisposable();

        MessageBrokerHolder.GameActions
            .Receive<M_PlayEffectByType>()
            .Subscribe((message) =>
                ShowEffects(message.EffectID, message.Transform))
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
    
    private void ShowEffects(EffectID effectID, Transform parent)
    {
        if (_spawners.TryGetValue(effectID, out var spawner))
        {
            var effect = spawner.Spawn();
            effect.transform.position = parent.position;
        }
        else
        {
            Debug.LogWarning($"No spawner found for EffectID: {effectID}");
        }
    }
}