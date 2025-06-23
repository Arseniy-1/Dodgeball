using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private float _stoppingDistance = 0.1f;
    private Coroutine _moveRoutine;
    private bool _canMove = false;

    private AreaPointSelector _pointSelector = new AreaPointSelector();

    public void WaypointMove(Collider squadZone, float speed)
    {
        Vector3 target = _pointSelector.GetRandomPointInZone(squadZone, transform.position);
        _moveRoutine = StartCoroutine(MoveTo(target, speed, squadZone));
    }

    private IEnumerator MoveTo(Vector3 target, float speed, Collider squadZone)
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

        if (_canMove)
            WaypointMove(squadZone, speed);
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