using System;
using Newtonsoft.Json;
using Project.Scripts.Rank;
using UnityEngine;
using YG;

namespace Project.Scripts.SavesSystem
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class Saves : MonoBehaviour
    {
        [SerializeField] private SavesYG _saves;
    
        public void Initialize(RankHolder rankHolder)
        {
            var startAnimationsData = _saves.StartAnimationsData;
        
            _saves = YG2.saves;
            _saves.InitializeStartSaves(startAnimationsData, rankHolder);
        }

        public void ResetProgress()
        {
            _saves.ResetProgress();
        }
    }
}