using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] private List<Entity> _entities;
    [SerializeField]private Ball _ball;

    private Collider _squadZone;
    
    private void Awake()
    {
        _squadZone = GetComponent<Collider>();

        foreach (var entity in _entities)
        {
            entity.Initialize(_squadZone, _entities, _ball);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
            MessageBrokerHolder.GameActions.Publish(new M_BallChangedZone(_squadZone));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Ball ball))
            MessageBrokerHolder.GameActions.Publish(new M_BallChangedZone(_squadZone));
    }
}