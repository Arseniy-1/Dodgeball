using Project.Scripts.Messages;
using UnityEngine;

namespace Project.Scripts.Services.EffectService
{
    public static class EffectExtensions
    {
        public static void PlayEffect(this EffectID effectID, Transform transform, bool isParent = false)
        {
            MessageBrokerHolder.GameActions.Publish(new M_PlayEffectByType(effectID, transform, isParent));
        }
    }
}