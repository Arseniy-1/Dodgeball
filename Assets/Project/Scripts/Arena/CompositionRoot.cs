using UnityEngine;
using System.Collections.Generic;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private List<Arena> _arenaPrefabs;

    private Arena _arenaInstance;

    private void Start()
    {
        StartGame();
    }
    
    private void StartGame()
    {
        if (_arenaInstance != null)
        {
            _arenaInstance.GameOver -= StartGame;   
            Destroy(_arenaInstance.gameObject);
        }
        
        Arena arenaPrefab = _arenaPrefabs[Random.Range(0, _arenaPrefabs.Count)];
        
        _arenaInstance = Instantiate(arenaPrefab, transform.position, Quaternion.identity);
    
        _arenaInstance.GameOver += StartGame;
        
        _arenaInstance.StartGame();
    }
    
}