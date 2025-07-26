using System.Collections.Generic;
using UnityEngine;
using Project.Scripts.Entities;

namespace Project.Scripts.Services
{
    public class TargetScanner : MonoBehaviour
    {
        public Entity Scan(List<Entity> teammates)
        {
            Collider[] colliderBuffer = new Collider[256];
            
            float scanRadius = 500f;
            float closestDistance = float.MaxValue;
            Entity closestEnemy = null;

            int hitsCount = Physics.OverlapSphereNonAlloc(
                transform.position, 
                scanRadius, 
                colliderBuffer);

            for (int i = 0; i < hitsCount; i++)
            {
                if (colliderBuffer[i].TryGetComponent(out Entity entity))
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