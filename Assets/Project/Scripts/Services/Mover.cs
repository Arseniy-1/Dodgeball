using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private float _stoppingDistance = 0.1f;
    private Coroutine _moveRoutine;
    private bool _canMove = false;

    public IEnumerator MoveTo(Vector3 target, float speed)
    {
        _canMove = true;

        while (Vector3.Distance(transform.position, target) > _stoppingDistance && _canMove)
        {
            Vector3 direction = (target - transform.position);
            direction.y = 0;

            transform.position = Vector3.MoveTowards(
                transform.position,
                transform.position + direction,
                speed * Time.deltaTime
            );

            yield return null;
        }
    }


    public void Stop()
    {
        _canMove = false;
        
        if (_moveRoutine != null)
        {
            StopCoroutine(_moveRoutine);
            _moveRoutine = null;
        }
    }
}