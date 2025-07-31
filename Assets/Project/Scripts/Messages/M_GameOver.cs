namespace Project.Scripts.Messages
{
    public struct M_GameOver
    {
        public M_GameOver(bool isPlayerWin)
        {
            IsPlayerWin = isPlayerWin;
        }

        public bool IsPlayerWin { get; private set; }
    }
}