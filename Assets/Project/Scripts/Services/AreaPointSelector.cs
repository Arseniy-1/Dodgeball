using UnityEngine;
using Random = UnityEngine.Random;

public class AreaPointSelector
{
    public Vector3 GetRandomPointInZone(Collider squadZone, Vector3 entityPosition)
    {
        Bounds bounds = squadZone.bounds;
        Vector3 randomPoint;
        
        int maxAttempts = 100;
        int attempts = 0;

        do
        {
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float z = Random.Range(bounds.min.z, bounds.max.z);
            float y = entityPosition.y;

            randomPoint = new Vector3(x, y, z);
            attempts++;

        } while (IsPointInsideCollider(squadZone, randomPoint) == false && attempts < maxAttempts);

        return randomPoint;
    }

    private bool IsPointInsideCollider(Collider col, Vector3 point)
    {
        Collider[] hits = Physics.OverlapSphere(point, 0.1f);

        foreach (var hit in hits)
        {
            if (hit == col)
                return true;
        }
        
        return false;
    }
}