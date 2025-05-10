using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

public class Squad : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<Entity> _entities;
    [SerializeField] private Ball _ball;

    public List<Transform> SpawnPoints => _spawnPoints;
    
    private Collider _squadZone;
    private CompositeDisposable _disposable = new CompositeDisposable();

    public event Action LostPlayers;
    
    private void Awake()
    {
        _squadZone = GetComponent<Collider>();
    }

    public void Initialize(List<Entity> entities)
    {
        _entities = entities;
        
        MessageBrokerHolder.GameActions.Receive<M_EntityDeath>()
            .Subscribe(message => HandleEntityDeath(message.Entity))
            .AddTo(_disposable);
        
        foreach (var entity in _entities)
            entity.Initialize(_squadZone, _entities, _ball);
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

    private void HandleEntityDeath(Entity entity)
    {
        if (_entities.Contains(entity))
            _entities.Remove(entity);
        
        if(_entities.Count == 0)
            LostPlayers?.Invoke();
    }
}