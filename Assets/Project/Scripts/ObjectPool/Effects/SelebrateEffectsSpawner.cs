using System;
using UniRx;
using UnityEngine;

[Serializable]
public class SelebrateEffectsSpawner : EffectsSpawner
{
    public SelebrateEffectsSpawner(Effect effect) : base(effect)
    {
        MessageBrokerHolder.GameActions.Receive<M_GameOver>()
            .Subscribe((message) => 
                HandleGameOver())
            .AddTo(CompositeDisposable);
    }

    private void HandleGameOver()
    {
        var effect = Spawn();
        effect.transform.position = Vector3.zero;
    }
}