using Sirenix.OdinInspector;
using UnityEngine;
using YG;

public class SaveTester : MonoBehaviour
{
    [Button]
    public void SaveProgress()
    {
        YG2.SaveProgress();
    }
    
    [Button]
    public void AddDodgeAnimation(Constans.DodgeAnimations dodgeAnimation)
    {
        YG2.saves.AnimationsHolder.AddDodgeAnimation(dodgeAnimation);
        YG2.SaveProgress();
    }
    
    [Button]
    public void Reset()
    {
        YG2.SetDefaultSaves();
        YG2.SaveProgress();
    }
}