using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Config/EnemyConfig", order = 51)]
public class EnemyConfig : EntityConfig
{
    [field: SerializeField] public float MinThrowWait { get; private set; }
    [field: SerializeField] public float MaxThrowWait { get; private set; }

    [field: SerializeField] public float DodgeJumpDelayMinTime { get; private set; }
    [field: SerializeField] public float DodgeJumpDelayMaxTime { get; private set; }
}