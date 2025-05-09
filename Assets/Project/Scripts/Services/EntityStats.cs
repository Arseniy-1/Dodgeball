using UnityEngine;

public class EntityStats : ScriptableObject, IThrowerStats
{
    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float RunSpeed { get; private set; }
    [field: SerializeField] public float RotationSpeed { get; private set; }
    [field: SerializeField] public float DodgeSpeed { get; private set; }
    
    [field: SerializeField] public float MinThrowForce { get; private set; }
    [field: SerializeField] public float MaxThrowForce { get; private set; }
    [field: SerializeField] public float ThrowChargeTime { get; private set; }
    
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float JumpStunTime { get; private set; }
    
    [field: SerializeField] public float DodgeDirectionChangeMinTime { get; private set; }
    [field: SerializeField] public float DodgeDirectionChangeMaxTime { get; private set; }
    
    [field: SerializeField] public float IdleMinStandTime { get; private set; }
    [field: SerializeField] public float IdleMaxStandTime { get; private set; }
}