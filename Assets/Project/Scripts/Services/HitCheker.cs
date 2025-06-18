using System;
using UnityEngine;

public class HitCheker : MonoBehaviour
{
    public event Action DetectBallHit;

    private void OnTriggerEnter(Collider other)
    {
        DetectBallHit?.Invoke();
    }
}