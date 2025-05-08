using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private StateMashine _stateMashine;

    public void Initialize(StateMashine stateMashine)
    {
        _stateMashine = stateMashine;
    }
    
    private void Update()
    {
        _stateMashine.Update();
    }
}
