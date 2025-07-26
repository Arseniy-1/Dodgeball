using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Project.Scripts.UpgradeFrame;
using Project.Scripts.UpgradeFrame.BallUpdaters;
using Random = UnityEngine.Random;

namespace Project.Scripts.CompositionRootSystem
{
    public class FrameActivator : IDisposable
    {
        private readonly List<Frame> _frames;
        private readonly float _minInterval;
        private readonly float _maxInterval;
        
        private CancellationTokenSource _cancellationTokenSource;
        private List<BallUpgrade> _currentUpgraders;

        public FrameActivator(List<Frame> frames, float minInterval, float maxInterval)
        {
            _frames = frames;
            _minInterval = minInterval;
            _maxInterval = maxInterval;
        }

        public void Initialize(List<BallUpgrade> upgraders)
        {
            _currentUpgraders = upgraders;
            _cancellationTokenSource = new CancellationTokenSource();
            ActivateFramesLoop().Forget();
        }
        
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        private async UniTaskVoid ActivateFramesLoop()
        {
            while (_cancellationTokenSource.IsCancellationRequested == false)
            {
                await ActivateRandomFrame();
                float delay = Random.Range(_minInterval, _maxInterval);
                await UniTask.Delay((int)(delay * 1000), cancellationToken: _cancellationTokenSource.Token);
            }
        }

        private async Task ActivateRandomFrame()
        {
            var frame = _frames[Random.Range(0, _frames.Count)];
            var upgrade = _currentUpgraders[Random.Range(0, _currentUpgraders.Count)];
            
            var taskCompletionSource = new TaskCompletionSource<bool>();
            
            void Handler(Frame _)
            {
                frame.OnFrameHit -= Handler;
                taskCompletionSource.SetResult(true);
            }

            frame.OnFrameHit += Handler;
            frame.Activate(upgrade);
            
            await taskCompletionSource.Task;
        }
    }
}