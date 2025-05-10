using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
    [SerializeField] private Arena _arena;

    private void Start()
    {
        _arena.StartGame();
    }
}