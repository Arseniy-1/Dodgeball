using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Effect : MonoBehaviour, IDestoyable<Effect>
{
    [SerializeField] private List<ParticleSystem> _particles;

    public event Action<Effect> OnDestroyed;

    private async void OnEnable()
    {
        foreach (var particle in _particles)
        {
            particle.Play();
        }

        await UniTask.WhenAll(_particles.Select(WaitForParticleAsync));

        Die();
    }
 
    public void Die()
    {
        OnDestroyed?.Invoke(this);
    }

    private async UniTask WaitForParticleAsync(ParticleSystem particle)
    {
        while (particle.IsAlive(true))
        {
            await UniTask.Yield();
        }
    }
}