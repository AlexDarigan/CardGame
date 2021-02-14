namespace CardGame.Server
{
    public class Player
    {
        public readonly int Id;
        public Player Opponent;
        public bool Ready = false;

        public Player(int id)
        {
            Id = id;
        }
    }
}