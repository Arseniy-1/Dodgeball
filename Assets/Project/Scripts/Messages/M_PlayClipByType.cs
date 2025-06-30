public struct M_PlayClipByType
{
    public AudioID AudioID { get; private set; }

    public M_PlayClipByType(AudioID audioID)
    {
        AudioID = audioID;
    }
}