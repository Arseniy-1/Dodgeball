using UnityEngine;

public class BallHolder : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private Transform _hand;

    public Ball LostBall()
    {
        _ball.transform.parent = null;
        _ball.Rigidbody.useGravity = true;
        _ball.Rigidbody.isKinematic = false;

        Ball returnedBall = _ball;
        _ball = null;

        return returnedBall;
    }


    public void EquipBall(Ball ball)
    {
        ball.transform.position = _hand.transform.position;
        ball.transform.parent = transform;

        ball.Rigidbody.velocity = Vector3.zero;
        ball.Rigidbody.angularVelocity = Vector3.zero;
        ball.Rigidbody.useGravity = false;
        ball.Rigidbody.isKinematic = true;
        
        _ball = ball;
    }
}