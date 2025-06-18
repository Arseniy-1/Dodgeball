using System;
using UnityEngine;

public class HitCheker : MonoBehaviour
{
    public event Action DetectBallHit;

    private void Start()
    {
        //Метод для жизненного цикла юнити
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Chargeable chargeable))
            if(chargeable.IsCharged)
                DetectBallHit?.Invoke();
    }
}