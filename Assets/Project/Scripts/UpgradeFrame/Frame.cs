using UnityEngine;
using System.Collections.Generic;
using System;
using Sirenix.OdinInspector;

public class Frame : MonoBehaviour
{
    [SerializeField] private FrameView _frameView; 
    [SerializeField] private Collider _collider;
    
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField] private float _speed = 3;
    
    private BallUpgrader _ballUpgrader;
    
    private int _currentWaypoint = 0;

    public event Action<Frame> OnFrameHitted;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ball ball))
        {
             _ballUpgrader.UpgradeBall(ball);                        
             HandleBallHit();
        }
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
        _frameView.gameObject.SetActive(transform);

        _ballUpgrader = ballUpgrader;
        _frameView.Initialize(_ballUpgrader.BallUpgradeInfo);
    }

    [Button]
    private void HandleBallHit()
    {
        _collider.enabled = false;
        _frameView.gameObject.SetActive(false);
        
        OnFrameHitted?.Invoke(this);
    }
}