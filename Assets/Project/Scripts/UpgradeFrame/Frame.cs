using UnityEngine;
using System.Collections.Generic;

public class Frame : MonoBehaviour
{
    [SerializeField] private FrameView _frameView; 
    [SerializeField] private Collider _collider;
    
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private float _speed = 3;
    
    private BallUpgrader _ballUpgrader;
    
    private int _currentWaypoint = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
            _ballUpgrader.UpgradeBall(ball);                        
    }
    
    private void Update()
    {
        if (transform.position == _waypoints[_currentWaypoint].position)
            _currentWaypoint = (_currentWaypoint + 1) % _waypoints.Count;

        transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypoint].position, _speed * Time.deltaTime);
    }
    
    public void Activate(BallUpgrader ballUpgrader)
    {
        _collider.enabled = true;
        _frameView.enabled = true;
        
        _frameView.Initialize(ballUpgrader.BallUpgradeInfo);
    }

    private void HandleBallHit()
    {
        _collider.enabled = false;
        _frameView.enabled = false;
    }
}