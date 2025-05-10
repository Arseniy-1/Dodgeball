using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public event Action<Ball> BallDetected;
    public event Action<int> DamageTaken;

    private void Start()
    {
        // Метод нужен, чтобы была возможность выключать компонент
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(gameObject.name + " collided with " + collision.gameObject.name);

        if (enabled == false)
            return;

        if (collision.collider.TryGetComponent(out Chargeable chargeable))
        {
            if (chargeable.IsCharged)
            {
                if (chargeable.TryGetComponent(out Damageable damageable))
                {
                    DamageTaken?.Invoke(damageable.Damage);
                    
                    return;
                }
            }
        }

        if (collision.collider.TryGetComponent(out Ball interactable))
            InteractWithBall(interactable);
    }

    private void InteractWithBall(Ball ball)
    {
        MessageBrokerHolder.GameActions.Publish(new M_BallTaken(transform.position));
        BallDetected?.Invoke(ball);
    }
}