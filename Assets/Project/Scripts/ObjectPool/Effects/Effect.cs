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

        try
        {
            await WaitForAllParticlesAsync(_cancellationToken.Token);
            Die();
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void OnDisable()
    {
        _cancellationToken?.Cancel();
    }

    public void Die()
    {
        if (transform != null)
            transform.parent = null;
        
        OnDestroyed?.Invoke(this);
    }

    private async UniTask WaitForAllParticlesAsync(CancellationToken cancellationToken)
    {
        var tasks = new List<UniTask>();

        foreach (var particle in _particles)
        {
            tasks.Add(WaitForParticleAsync(particle, cancellationToken));
        }

        if (cancellationToken.IsCancellationRequested)
            return;

        await UniTask.WhenAll(tasks);
    }

    private async UniTask WaitForParticleAsync(ParticleSystem particle, CancellationToken cancellationToken)
    {
        await UniTask.NextFrame(cancellationToken: cancellationToken);

        if (cancellationToken.IsCancellationRequested)
            return;

        while (cancellationToken.IsCancellationRequested == false && particle.IsAlive(true))
        {
            await UniTask.Yield();
        }
    }
}