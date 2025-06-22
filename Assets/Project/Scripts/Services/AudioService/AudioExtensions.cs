public static class AudioExtensions
{
    public static void PlayOneShot(this AudioID audioID)
    {
        MessageBrokerHolder.GameActions.Publish(new M_PlayClipByType(audioID));
    }
}