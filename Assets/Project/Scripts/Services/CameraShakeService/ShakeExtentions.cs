using YG;

public static class ShakeExtentions
{
    public static void Play(this ShakeID effectID)
    {
        if (YG2.saves.SettingsData.IsCameraShakeEnabled == false)
            return;
        
        MessageBrokerHolder.GameActions.Publish(new M_CameraShake(effectID));
    }
}