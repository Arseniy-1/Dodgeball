using Project.Scripts.Messages;
using YG;

namespace Project.Scripts.Services.AudioServiceSystem
{
    public static class AudioExtensions
    {
        public static void PlayOneShot(this AudioID audioID)
        {
            if (YG2.saves.SettingsData.IsSoundsEnabled == false)
                return;
        
            MessageBrokerHolder.GameActions.Publish(new M_PlayClipByType(audioID));
        }
    }
}