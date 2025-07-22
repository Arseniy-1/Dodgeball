using Project.Scripts.Entities;
using UnityEngine;

namespace Project.Scripts.Services.Ball
{
    public class BallHolder : MonoBehaviour
    {
        [SerializeField] private Scripts.Ball _ball;
        [SerializeField] private Transform _hand;

        public Scripts.Ball LostBall()
        {
            if (_ball != null)
            {
                _ball.transform.parent = null;
                _ball.Rigidbody.useGravity = true;
                _ball.Rigidbody.isKinematic = false;
                GameStatusService.Instance.ClearHolder();
            }

            Scripts.Ball returnedBall = _ball;
            _ball = null;
        
            return returnedBall;
        }

        public void EquipBall(Scripts.Ball ball, Entity owner)
        {
            GameStatusService.Instance.SetHolder(owner);   
        
            ball.transform.position = _hand.transform.position;
            ball.transform.parent = transform;

            ball.Rigidbody.velocity = Vector3.zero;
            ball.Rigidbody.angularVelocity = Vector3.zero;
            ball.Rigidbody.useGravity = false;
            ball.Rigidbody.isKinematic = true;

            _ball = ball;
        }
    }
}