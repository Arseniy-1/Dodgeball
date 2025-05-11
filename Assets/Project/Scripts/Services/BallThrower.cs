using System.Collections;
using UnityEngine;
using System;

public class BallThrower : MonoBehaviour
{
    [SerializeField] private float _currentForce;
    
    private IThrowerStats _throwerStats;
    private Coroutine _charging;

    public event Action<float, float, float> OnCharging;
    public event Action OnThrown;

    public void Initialize(IThrowerStats throwerStats)
    {
        _throwerStats = throwerStats;
    }
    
    public void StartCharging()
    {
        _currentForce = _throwerStats.MinThrowForce;
        _charging = StartCoroutine(Charging());
    }

    public void StopCharging()
    {
        if (_charging != null)
            StopCoroutine(_charging);
    }

    public void Throw(Ball ball)
    {
        if(ball == null) 
            return;
        
        ball.Rigidbody.AddForce(transform.forward * _currentForce, ForceMode.Force);
        
        _currentForce = _throwerStats.MinThrowForce;
        
        OnCharging?.Invoke(_currentForce, _throwerStats.MinThrowForce, _throwerStats.MaxThrowForce);
        OnThrown?.Invoke();
    }

    private IEnumerator Charging()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _throwerStats.ThrowChargeTime)
        {
            elapsedTime += Time.deltaTime;
            _currentForce = Mathf.Lerp(_throwerStats.MinThrowForce, _throwerStats.MaxThrowForce, elapsedTime / _throwerStats.ThrowChargeTime);
         
            OnCharging?.Invoke(_throwerStats.MinThrowForce, _throwerStats.MaxThrowForce, _currentForce);

            yield return null;
        }

        _currentForce = _throwerStats.MaxThrowForce;
    }
}