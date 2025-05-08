using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public event Action<Ball> BallDetected;

    private void Start()
    {
    }

    // private void OnTriggerEnter(Collider collision)
    // {
    //     if (collision.TryGetComponent(out Ball interactable))
    //     {
    //         Interact(interactable);
    //     }
    // }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Ball interactable))
        {
            Interact(interactable);
        }
    }

    private void Interact(Ball ball)
    {
        MessageBrokerHolder.GameActions.Publish(new M_BallTaken(transform.position));
        BallDetected?.Invoke(ball);
    }
}