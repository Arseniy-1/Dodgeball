using UnityEngine;

public class Rotator
{
    public void RotateToTarget(Transform target, Transform holder, float rotationSpeed)
    {
        Vector3 direction = (target.transform.position - holder.transform.position);
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            holder.transform.rotation = 
                Quaternion.RotateTowards(holder.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    public void RotateToTarget(Transform target, Transform holder)
    {
        Vector3 direction = (target.position - holder.position);
        direction.y = 0;
    
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            holder.rotation = targetRotation;
        }
    }
}