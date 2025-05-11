using System.Collections.Generic;
using UnityEngine;

public class TargetScanner : MonoBehaviour
{
    public Entity Scan(List<Entity> teammates)
    {
        float scanRadius = 500f;
        float closestDistance = float.MaxValue;
        Entity closestEnemy = null;

        Collider[] colliders = Physics.OverlapSphere(transform.position, scanRadius);

        foreach (Collider col in colliders)
        {
            Debug.Log(col.gameObject.name);
            if (col.TryGetComponent(out Entity entity))
            {
                Debug.Log("enemy" + col.gameObject.name);
                if (entity == null || teammates.Contains(entity)) continue;

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