using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Services
{
    public class AreaPointSelector
    {
        private Collider[] _colliderBuffer = new Collider[5];
        
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

            } 
            while (IsPointInsideCollider(squadZone, randomPoint) == false && attempts < maxAttempts);

            return randomPoint;
        }
        
        private bool IsPointInsideCollider(Collider col, Vector3 point)
        {
            const float radius = 0.1f;
            
            int hitsCount = Physics.OverlapSphereNonAlloc(
                point, 
                radius, 
                _colliderBuffer
            );

            for (int i = 0; i < hitsCount; i++)
            {
                if (_colliderBuffer[i] == col)
                    return true;
            }
    
            return false;
        }
    }
}