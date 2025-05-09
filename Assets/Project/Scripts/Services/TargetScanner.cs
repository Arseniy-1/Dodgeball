using System.Collections.Generic;
using UnityEngine;

public class TargetScanner : MonoBehaviour
{
    public Entity Scan(List<Entity> teammates)
    {
        float scanRadius = 50f;
        float closestDistance = float.MaxValue;
        Entity closestEnemy = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, scanRadius);

        foreach (Collider col in colliders)
        {
            Entity entity = col.GetComponent<Entity>();
            if (entity == null || teammates.Contains(entity)) continue;

            float distance = Vector3.Distance(transform.position, entity.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = entity;
            }
        }

        return closestEnemy;
    }
}