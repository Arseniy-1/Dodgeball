using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

public class Squad : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<Entity> _entities;
    [SerializeField] private Ball _ball;

    public Type SquadType => _entities[0].GetType();
    public List<Transform> SpawnPoints => _spawnPoints;

    private Collider _squadZone;
    private CompositeDisposable _disposable = new CompositeDisposable();

    public event Action<Squad> LostPlayers;

    private void Awake()
    {
    }

    private void OnDestroy()
    {
        foreach (var entity in _entities)
        {
            if (entity is Enemy enemy)
                enemy.OnDestroyed -= HandleEntityDeath;
            else if (entity is Player player)
                player.OnDestroyed -= HandleEntityDeath;
        }
    }

    public void Initialize(List<Entity> entities, Ball ball)
    {
        _squadZone = GetComponent<Collider>();
        
        _entities = entities;
        _ball = ball;

        foreach (var entity in _entities)
        {
            entity.Initialize(_squadZone, _entities, _ball);
            
            if (entity is Enemy enemy)
                enemy.OnDestroyed += HandleEntityDeath;
            else if (entity is Player player)
                player.OnDestroyed += HandleEntityDeath;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            if (ball.Rigidbody.isKinematic)
                return;

            MessageBrokerHolder.GameActions.Publish(new M_BallChangedZone(_squadZone));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Ball ball))
            MessageBrokerHolder.GameActions.Publish(new M_BallChangedZone(_squadZone));
    }

    private void HandleEntityDeath(Entity entity)
    {
        if (_entities.Contains(entity))
            _entities.Remove(entity);

        if (_entities.Count == 0)
            LostPlayers?.Invoke(this);
    }
}