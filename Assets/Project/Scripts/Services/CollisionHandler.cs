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
        Debug.Log(gameObject.name + " collided with " + collision.gameObject.name);
        if (enabled == false)
        {
            return;
        }

        if (collision.collider.TryGetComponent(out Ball interactable))
            Interact(interactable);
    }

    private void Interact(Ball ball)
    {
        MessageBrokerHolder.GameActions.Publish(new M_BallTaken(transform.position));
        BallDetected?.Invoke(ball);
    }
}