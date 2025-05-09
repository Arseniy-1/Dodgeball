using System.Collections.Generic;
using UnityEngine;
public class Enemy : Entity
{
    [SerializeField] private EnemyStats _enemyStats;

    public override void Initialize(Collider squadZone, List<Entity> teamates, Ball ball)
    {
        base.Initialize(squadZone, teamates, ball);
        BallThrower.Initialize(_enemyStats);
        List<IState> enemyStats = new List<IState>
        {
            new EnemyIdleState(this, CollisionHandler, SquadZone, BallHolder, CompositeDisposable)
        };

        StateMashine = new StateMashine(enemyStats);

        foreach (var state in enemyStats)
            state.Initialize(StateMashine);
    }

}