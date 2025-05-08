using UnityEngine;

[CreateAssetMenu(fileName = "EntityStats", menuName = "Stats/EntityStats", order = 51)]
public class EntityStats : ScriptableObject, IThrowerStats
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public float DodgeSpeed { get; private set; }

    [field: SerializeField] public float MinThrowForce { get; private set; }
    [field: SerializeField] public float MaxThrowForce { get; private set; }
    [field: SerializeField] public float ThrowChargeTime { get; private set; }
    [field: SerializeField] public float MinThrowWait { get; private set; }
    [field: SerializeField] public float MaxThrowWait { get; private set; }

    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float JumpStunTime { get; private set; }

    [field: SerializeField] public float DodgeDirectionChangeMinTime { get; private set; }
    [field: SerializeField] public float DodgeDirectionChangeMaxTime { get; private set; }
    [field: SerializeField] public float DodgeJumpDelayMinTime { get; private set; }
    [field: SerializeField] public float DodgeJumpDelayMaxTime { get; private set; }

    [field: SerializeField] public float IdleMinStandTime { get; private set; }
    [field: SerializeField] public float IdleMaxStandTime { get; private set; }
}

public interface IThrowerStats
{
    float MinThrowForce { get; }
    float MaxThrowForce { get; }
    float ThrowChargeTime { get; }
}