using System;
using System.Collections.Generic;
using Project.Scripts.Messages;
using Project.Scripts.ObjectPool;
using Project.Scripts.ObjectPool.Effects;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Project.Scripts.Services.EffectServiceSystem
{
    [Serializable]
    public class EffectService : IDisposable
    {
        [OdinSerialize] private Dictionary<EffectID, EffectData> _effectsData;

        private Dictionary<EffectID, List<Spawner<Effect>>> _spawners;
        private CompositeDisposable _compositeDisposable;

        public EffectService(Dictionary<EffectID, EffectData> effectsData)
        {
            _effectsData = effectsData;
            InitializePool();
            SubscribeToMessages();
        }
        
        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        private void InitializePool()
        {
            var poolHolder = new GameObject("EffectsPoolHolder");
            Object.DontDestroyOnLoad(poolHolder);
            
            _spawners = new Dictionary<EffectID, List<Spawner<Effect>>>();

            foreach (var pair in _effectsData)
            {
                var spawners = new List<Spawner<Effect>>();
                
                foreach (var effect in pair.Value.Effects)
                {
                    spawners.Add(new Spawner<Effect>(effect, poolHolder.transform));
                }
                
                _spawners[pair.Key] = spawners;
            }
        }

        private void SubscribeToMessages()
        {
            _compositeDisposable = new CompositeDisposable();
            
            MessageBrokerHolder.GameActions
                .Receive<M_PlayEffectByType>()
                .Subscribe(ShowEffect)
                .AddTo(_compositeDisposable);
        }

        private void ShowEffect(M_PlayEffectByType message)
        {
            if (_spawners.TryGetValue(message.EffectID, out var spawners) == false)
                return;

            var randomSpawner = spawners[Random.Range(0, spawners.Count)];
            var effect = randomSpawner.Spawn();
            
            effect.transform.position = message.Transform.position;

            if (message.IsParent)
                effect.transform.parent = message.Transform;
        }
    }
}