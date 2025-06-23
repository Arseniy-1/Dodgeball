using UnityEngine;

public class EffectsPool: Pool<Effect>
{
    private Transform _parent;
    
    public EffectsPool(Effect prefab, int startAmount, Transform parent) : base(prefab, startAmount)
    {
        _parent = parent;
    }
        
    protected override Effect Create()
    {
        var effect =  Object.Instantiate(Prefab, _parent);
        effect.gameObject.SetActive(false);
            
        return effect;
    }
}