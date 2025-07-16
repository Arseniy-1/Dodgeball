using System.Collections.Generic;
using System;
using UnityEngine;

public class Squad : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<Entity> _entities;

    public Type SquadType => _entities[0].GetType();
    public List<Transform> SpawnPoints => _spawnPoints;
    public Collider SquadZone => _squadZone;

    [SerializeField] private Collider _squadZone;

    public event Action<Squad> LostPlayers;

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

    public void Initialize(List<Entity> entities)
    {
        _entities = entities;

        foreach (var entity in _entities)
        {
            if (entity is Enemy enemy)
                enemy.OnDestroyed += HandleEntityDeath;
            else if (entity is Player player)
                player.OnDestroyed += HandleEntityDeath;
        }
    }

    public void Celebrate()
    {
        foreach (var entity in _entities)
        {
            entity.Celebrate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            // if (ball.Rigidbody.isKinematic)
            //     return;
            //
            // if (GameStatusService.Instance.CurrentZone != null)
            //     return;
            //
            // Debug.Log("1");
            //
            // GameStatusService.Instance.SetCurrentZone(_squadZone);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            if (ball.Rigidbody.isKinematic)
                return;

            GameStatusService.Instance.ClearCurrentZone();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
            if (ball.Rigidbody.isKinematic)
                return;

            if (GameStatusService.Instance.CurrentZone != null)
                return;

            GameStatusService.Instance.SetCurrentZone(_squadZone);
        }
    }

    private void HandleEntityDeath(Entity entity)
    {
        if (_entities.Contains(entity))
            _entities.Remove(entity);

        if (_entities.Count == 0)
            LostPlayers?.Invoke(this);
    }
}