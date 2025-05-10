using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private List<Squad> _squads;

    public void StartGame()
    {
        foreach (var squad in _squads)
        {
            squad.Initialize();
        }
    }
}