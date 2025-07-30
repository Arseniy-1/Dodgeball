using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Project.Scripts.UpgradeFrame;
using Project.Scripts.UpgradeFrame.BallUpdaters;
using Random = UnityEngine.Random;

namespace Project.Scripts.GameSystem
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
        }

        public void Activate()
        {
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
                await ActivateFrame();
                float delay = Random.Range(_minInterval, _maxInterval);
                await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: _cancellationTokenSource.Token);
            }
        }

        private async Task ActivateFrame()
        {
            var frame = _frames[Random.Range(0, _frames.Count)];
            var upgrade = _currentUpgraders[Random.Range(0, _currentUpgraders.Count)];
            
            var taskCompletionSource = new TaskCompletionSource<bool>();
            
            void OnFrameHit(Frame _)
            {
                frame.FrameHit -= OnFrameHit;
                taskCompletionSource.SetResult(true);
            }

            frame.FrameHit += OnFrameHit;
            frame.Activate(upgrade);
            
            await taskCompletionSource.Task;
        }
    }
}