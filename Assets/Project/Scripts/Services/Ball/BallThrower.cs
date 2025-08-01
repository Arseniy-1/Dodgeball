using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Scripts.Services.Ball
{
    public class BallThrower : MonoBehaviour
    {
        [SerializeField] private float _currentForce;

        private IThrowerStats _throwerStats;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isCharging = true;
        private float _chargeProgress;

        public event Action<float, float, float> Charging;
        public event Action Thrown;

        public void Initialize(IThrowerStats throwerStats)
        {
            _throwerStats = throwerStats;
        }

        public void StartCharging()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            _currentForce = _throwerStats.MinThrowForce;
            _isCharging = true;
            _chargeProgress = 0f;

            ChargingCycle(_cancellationTokenSource.Token).Forget();
        }

        public void StopCharging()
        {
            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();
        }

        public void Throw(Ball ball)
        {
            if (ball == null)
                return;

            ball.Rigidbody.AddForce(transform.forward * _currentForce, ForceMode.Force);
            Thrown?.Invoke();
        }

        private async UniTaskVoid ChargingCycle(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                if (_isCharging)
                {
                    _chargeProgress += Time.deltaTime / _throwerStats.ThrowChargeTime;

                    if (_chargeProgress >= 1f)
                    {
                        _chargeProgress = 1f;
                        _isCharging = false;
                    }
                }
                else
                {
                    _chargeProgress -= Time.deltaTime / _throwerStats.ThrowChargeTime;

                    if (_chargeProgress <= 0f)
                    {
                        _chargeProgress = 0f;
                        _isCharging = true;
                    }
                }

                _currentForce = Mathf.Lerp(_throwerStats.MinThrowForce, _throwerStats.MaxThrowForce, _chargeProgress);

                Charging?.Invoke(_throwerStats.MinThrowForce, _throwerStats.MaxThrowForce, _currentForce);

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }
    }
}