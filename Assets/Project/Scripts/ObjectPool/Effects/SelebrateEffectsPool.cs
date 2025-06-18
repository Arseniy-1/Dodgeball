using UnityEngine;

public class SelebrateEffectsPool: Pool<Effect>
{
    public SelebrateEffectsPool(Effect prefab, int startAmount) : base(prefab, startAmount)
    {
    }
        
    protected override Effect Create()
    {
        var effect =  Object.Instantiate(Prefab);
        effect.gameObject.SetActive(false);
            
        return effect;
    }
}