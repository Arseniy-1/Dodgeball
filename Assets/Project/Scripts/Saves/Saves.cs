using UnityEngine;
using UnityEngine.Serialization;
using YG;

public class Saves : MonoBehaviour
{
    [SerializeField] private SavesYG _saves;
    
    public void Initialize()
    {
        _saves.InitializeSaves();
    }
}