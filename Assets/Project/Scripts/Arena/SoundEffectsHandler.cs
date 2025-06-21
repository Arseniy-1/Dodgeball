using UniRx;
using UnityEngine;
using System.Collections.Generic;

public class SoundEffectsHandler : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _deathSounds;
    [SerializeField] private List<AudioClip> _hitSounds;
    [SerializeField] private List<AudioClip> _dodgeSounds;
    [SerializeField] private List<AudioClip> _jumpSounds;
    
    private CompositeDisposable _compositeDisposable = new CompositeDisposable();
    
    public void Initialize()
    {
        MessageBrokerHolder.GameActions
            .Receive<M_EntityDeath>()
            .Subscribe((message) => 
                HandleEntityDeath(message.Entity))
            .AddTo(_compositeDisposable);
        
        MessageBrokerHolder.GameActions
            .Receive<M_EntityDodged>()
            .Subscribe((message) => 
                HandleEntityDodge(message.EntityTransform))
            .AddTo(_compositeDisposable);
        
        MessageBrokerHolder.GameActions
            .Receive<M_EntityHited>()
            .Subscribe((message) => 
                HandleEntityHit(message.EntityTransform))
            .AddTo(_compositeDisposable);
    }

    private void HandleEntityDeath(Entity entity)
    {
        AudioClip randomSound = _deathSounds[Random.Range(0, _deathSounds.Count)];
        AudioSource.PlayClipAtPoint(randomSound, entity.transform.position);
    }
    
    private void HandleEntityDodge(Transform transform)
    {
        AudioClip randomSound = _dodgeSounds[Random.Range(0, _dodgeSounds.Count)];
        AudioSource.PlayClipAtPoint(randomSound, transform.position);
    }
    
    private void HandleEntityHit(Transform transform)
    {
        AudioClip randomSound = _hitSounds[Random.Range(0, _hitSounds.Count)];
        AudioSource.PlayClipAtPoint(randomSound, transform.position);
    }
}