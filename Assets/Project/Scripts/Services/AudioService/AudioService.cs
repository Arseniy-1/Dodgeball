using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.Serialization;
using UnityEngine;
using UniRx;
using UnityEngine.Pool;
using YG;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using AudioData = AudioSettings.AudioData;

public class AudioService : IDisposable
{
    [OdinSerialize] private Dictionary<AudioID, AudioData> _audioData;

    private ObjectPool<AudioSource> _audioPool;
    private CompositeDisposable _compositeDisposable;

    public AudioService(Dictionary<AudioID, AudioData> audioData)
    {
        _audioData = audioData;

        var poolHolder = new GameObject("AudioPoolHolder");
        Object.DontDestroyOnLoad(poolHolder);

        AudioSource audioSourse = new GameObject("AudioSourse").AddComponent<AudioSource>();

        _audioPool = new ObjectPool<AudioSource>(() => Object.Instantiate(audioSourse, poolHolder.transform));

        _compositeDisposable = new CompositeDisposable();

        MessageBrokerHolder.GameActions
            .Receive<M_PlayClipByType>()
            .Subscribe((message) =>
                PlaySound(message.AudioID))
            .AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }

    private void PlaySound(AudioID audioID)
    {
        if (YG2.saves.SettingsData.IsSoundsEnabled == false)
            return;
        
        AudioData data = _audioData[audioID];
        AudioClip randomClip = data.Clips[Random.Range(0, data.Clips.Count)];
        float pitch = Random.Range(data._pitchRange.x, data._pitchRange.y);

        AudioSource audioSource = _audioPool.Get();
        audioSource.volume = data.Volume;
        audioSource.pitch = pitch;

        audioSource.PlayOneShot(randomClip);
        ReleaseSourse(audioSource, _audioPool, randomClip.length).Forget();
    }

    private async UniTaskVoid ReleaseSourse(AudioSource source, ObjectPool<AudioSource> pool, float length = 1)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(length));
        pool.Release(source);
    }
}