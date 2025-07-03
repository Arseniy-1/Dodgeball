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

    private Dictionary<EffectID, List<EffectsSpawner>> _spawners;
    private CompositeDisposable _compositeDisposable;

    public EffectService(Dictionary<EffectID, EffectData> effectsData)
    {
        _effectsData = effectsData;

        var poolHolder = new GameObject("EffectsPoolHolder");
        Object.DontDestroyOnLoad(poolHolder);

        _spawners = new Dictionary<EffectID, List<EffectsSpawner>>();

        foreach (var pair in _effectsData)
        {
            List<EffectsSpawner> spawners = new List<EffectsSpawner>();
            
            foreach (var effect in pair.Value.Effects)
                spawners.Add(new EffectsSpawner(effect, poolHolder.transform));

            _spawners[pair.Key] = spawners;
        }

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
        if (_spawners.TryGetValue(effectID, out var spawners))
        {
            EffectsSpawner randomSpawner = spawners[UnityEngine.Random.Range(0, spawners.Count)];
            var effect = randomSpawner.Spawn();
            effect.transform.parent = parent.transform;
            effect.transform.position = parent.position;
        }
        else
        {
            Debug.LogWarning($"No spawner found for EffectID: {effectID}");
        }
    }
}