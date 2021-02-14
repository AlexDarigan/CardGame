using System.Collections.Generic;

namespace CardGame.Server
{
    public class Player
    {
        public readonly int Id;
        public Player Opponent;
        public bool Ready = false;
        public readonly List<Card> Deck = new List<Card>();

        public Player(int id)
        {
            Id = id;
        }
    }
}