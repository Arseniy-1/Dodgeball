using UnityEngine;

public static class EffectExtensions
{
    public static void PlayEffect(this EffectID effectID, Transform transform)
    {
        MessageBrokerHolder.GameActions.Publish(new M_PlayEffectByType(effectID, transform));
    }
}