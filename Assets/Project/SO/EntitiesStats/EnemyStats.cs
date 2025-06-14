using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Stats/EntityStats", order = 51)]
public class EnemyStats : EntityStats
{
    [field: SerializeField] public float MinThrowWait { get; private set; }
    [field: SerializeField] public float MaxThrowWait { get; private set; }

    [field: SerializeField] public float DodgeJumpDelayMinTime { get; private set; }
    [field: SerializeField] public float DodgeJumpDelayMaxTime { get; private set; }
}