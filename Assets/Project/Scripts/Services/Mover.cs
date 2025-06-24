using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private float _stoppingDistance = 0.1f;
    private Coroutine _moveRoutine;
    private bool _canMove = false;

    private float _stepDistanceThreshold = 1.2f;
    private float _distanceSinceLastStep = 0f;
    private Vector3 _lastPosition;

    public IEnumerator MoveTo(Vector3 target, float speed)
    {
        _canMove = true;
        _lastPosition = transform.position;
        _distanceSinceLastStep = 0f;

        while (Vector3.Distance(transform.position, target) > _stoppingDistance && _canMove)
        {
            Vector3 direction = (target - transform.position);
            direction.y = 0;

            transform.position = Vector3.MoveTowards(
                transform.position,
                transform.position + direction,
                speed * Time.deltaTime
            );

            TrackStep();
            yield return null;
        }
    }

    public void FollowTarget(Transform target, float speed)
    {
        if (target == null) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.position;
        targetPos.y = currentPos.y;

        transform.position = Vector3.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);

        TrackStep();
    }

    private void TrackStep()
    {
        float moved = Vector3.Distance(transform.position, _lastPosition);
        _distanceSinceLastStep += moved;

        if (moved > 0.001f && _distanceSinceLastStep >= _stepDistanceThreshold)
        {
            AudioID.Walk.PlayOneShot();
            _distanceSinceLastStep = 0f;
        }

        _lastPosition = transform.position;
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