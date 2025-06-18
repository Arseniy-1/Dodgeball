using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class Effect : MonoBehaviour, IDestoyable<Effect>
{
    [SerializeField] private List<ParticleSystem> _particles;

    private CancellationTokenSource _cancellationToken;
    
    public event Action<Effect> OnDestroyed;

    private async void OnEnable()
    {
        _cancellationToken = new CancellationTokenSource();
        
        foreach (var particle in _particles)
            particle.Play();

        foreach (var particle in _particles)
            await WaitForParticleAsync(particle, _cancellationToken.Token);

        Die();
    }

    private void OnDisable()
    {
        _cancellationToken.Cancel();
    }
 
    public void Die()
    {
        OnDestroyed?.Invoke(this);
    }

    private async UniTask WaitForParticleAsync(ParticleSystem particle, CancellationToken cancellationToken)
    {
        while (cancellationToken.IsCancellationRequested == false && particle.IsAlive(true))
        {
            await UniTask.Yield();
        }
    }
}