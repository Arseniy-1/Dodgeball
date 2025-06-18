using UnityEngine;

public class HitEffectsPool: Pool<Effect>
{
    public HitEffectsPool(Effect prefab, int startAmount) : base(prefab, startAmount)
    {
    }
        
    protected override Effect Create()
    {
        var effect =  Object.Instantiate(Prefab);
        effect.gameObject.SetActive(false);
            
        return effect;
    }
}