using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class AudioService : SerializedMonoBehaviour
{
    [OdinSerialize] private Dictionary<AudioID, List<AudioClip>> _audioClips;
    [SerializeField] private AudioSource _audioSource;
    
    private ObjectPool<AudioSource> _audioPool;
    private CompositeDisposable _compositeDisposable;
    
    public void Initialize()
    {
        var poolHolder = new GameObject("AudioPoolHolder");
        Object.DontDestroyOnLoad(poolHolder);
        
        _audioPool = new ObjectPool<AudioSource>(() => Object.Instantiate(_audioSource, poolHolder.transform));
        
        _compositeDisposable = new CompositeDisposable();
        
        MessageBrokerHolder.GameActions
            .Receive<M_PlayClipByType>()
            .Subscribe((message) => 
                PlaySound(message.AudioID))
            .AddTo(_compositeDisposable);
    }

    private void PlaySound(AudioID audioID)
    {
        List<AudioClip> clips = _audioClips[audioID];
        AudioClip randomClip = clips[Random.Range(0, clips.Count)];
        
        AudioSource audioSource = _audioPool.Get();
        audioSource.PlayOneShot(randomClip);
        ReleaseSourse(audioSource, _audioPool, randomClip.length).Forget();
    }

    private async UniTaskVoid ReleaseSourse(AudioSource source, ObjectPool<AudioSource> pool, float length = 1)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(length));
        pool.Release(source);
    }
}