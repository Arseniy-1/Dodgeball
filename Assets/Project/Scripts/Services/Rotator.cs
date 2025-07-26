using UnityEngine;

namespace Project.Scripts.Services
{
    public class Rotator
    {
        public void RotateToTarget(Transform target, Transform holder, float rotationSpeed)
        {
            Quaternion targetRotation = CalculateRotation(target, holder);
            
            holder.transform.rotation =
                Quaternion.RotateTowards(holder.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        public void RotateToTarget(Transform target, Transform holder)
        {
            Quaternion targetRotation = CalculateRotation(target, holder);
            
            holder.rotation = targetRotation;
        }

        private Quaternion CalculateRotation(Transform target, Transform holder)
        {
            Vector3 direction = target.position - holder.position;
            direction.y = 0;

            return Quaternion.LookRotation(direction.normalized);
        }
    }
}