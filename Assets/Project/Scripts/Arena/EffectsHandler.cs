using UnityEngine;

public class EffectsHandler : MonoBehaviour
{
    [SerializeField] private DeathEffectsSpawner _deathEffectsSpawner;
    [SerializeField] private Effect _deathEffect;
    
    [SerializeField] private HitEffectsSpawner _hitEffectsSpawner;
    [SerializeField] private Effect _hitEffect;
    
    [SerializeField] private SelebrateEffectsSpawner _selebrateEffectsSpawner;
    [SerializeField] private Effect _selebrateEffect;
    
    [SerializeField] private DodgeEffectsSpawner _dodgeEffectsSpawner;
    [SerializeField] private Effect _dodgeEffect;

    public void Initialize()
    {
        _deathEffectsSpawner = new DeathEffectsSpawner(_deathEffect);
        _hitEffectsSpawner = new HitEffectsSpawner(_hitEffect);
        _selebrateEffectsSpawner = new SelebrateEffectsSpawner(_selebrateEffect);
        _dodgeEffectsSpawner = new DodgeEffectsSpawner(_dodgeEffect);
    }
}