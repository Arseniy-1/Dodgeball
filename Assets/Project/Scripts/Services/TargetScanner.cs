using System.Collections.Generic;
using UnityEngine;
using Project.Scripts.Entities;

namespace Project.Scripts.Services
{
    public class TargetScanner : MonoBehaviour
    {
        private Collider[] _colliderBuffer = new Collider[32];

        public Entity Scan(List<Entity> teammates)
        {
            float scanRadius = 500f;
            float closestDistance = float.MaxValue;
            Entity closestEnemy = null;

            int hitsCount = Physics.OverlapSphereNonAlloc(
                transform.position, 
                scanRadius, 
                _colliderBuffer
            );

            for (int i = 0; i < hitsCount; i++)
            {
                if (_colliderBuffer[i].TryGetComponent(out Entity entity))
                {
                    if (entity == null || teammates.Contains(entity)) 
                        continue;

                    float distance = Vector3.Distance(transform.position, entity.transform.position);
            
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = entity;
                    }
                }
            }

            return closestEnemy;
        }
    }
}