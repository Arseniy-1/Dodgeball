using Project.Scripts.Services.AudioServiceSystem;

namespace Project.Scripts.Messages
{
    public struct M_PlayClipByType
    {
        public M_PlayClipByType(AudioID audioID)
        {
            AudioID = audioID;
        }
     
        public AudioID AudioID { get; private set; }
    }
}