using System;
using UniRx;
using UnityEditor.VersionControl;
using UnityEngine;

[Serializable]
public class DodgeEffectsSpawner : EffectsSpawner
{
    public DodgeEffectsSpawner(Effect effect) : base(effect)
    {
        MessageBrokerHolder.GameActions
            .Receive<M_EntityDodged>()
            .Subscribe((message) => 
                HandleEntityDodge(message.EntityTransform))
            .AddTo(CompositeDisposable);
    }

    private void HandleEntityDodge(Transform transform)
    {
        var effect = Spawn();
        effect.transform.position = transform.position;
    }
}