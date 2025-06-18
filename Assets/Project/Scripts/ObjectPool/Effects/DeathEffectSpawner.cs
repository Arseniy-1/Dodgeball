using System;
using UniRx;
using UnityEngine;

[Serializable]
 public class DeathEffectsSpawner : EffectsSpawner
 {
     public DeathEffectsSpawner(Effect effect) : base(effect)
     {
         MessageBrokerHolder.GameActions.Receive<M_EntityDeath>()
             .Subscribe((message) => 
                 HandleEntityDeath(message.Entity))
             .AddTo(CompositeDisposable);
     }
 
     private void HandleEntityDeath(Entity entity)
     {
         var effect = Spawn();
         effect.transform.position = entity.transform.position;
     }
 }