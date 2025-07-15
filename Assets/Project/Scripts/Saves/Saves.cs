using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using YG;

[Serializable]
[JsonObject(MemberSerialization.Fields)]
public class Saves : MonoBehaviour
{
    [SerializeField] private SavesYG _saves;
    
    public void Initialize()
    {
        var startAnimationsData = _saves.StartAnimationsData;
        
        _saves = YG2.saves;
        _saves.InitializeStartSaves(startAnimationsData);
    }

    public void ResetProgress()
    {
        _saves.ResetAnimations();
        YG2.isFirstGameSession = true;
    }
}