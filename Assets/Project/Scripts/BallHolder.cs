using UnityEngine;

public class BallHolder : MonoBehaviour
{
    [SerializeField] private Ball _ball;
    [SerializeField] private Transform _hand;

    public Ball GetBall()
    {
        return _ball;
    }

    public void EquipBall(Ball ball)
    {
        ball.transform.position = _hand.transform.position;

        _ball = ball;
    }
}