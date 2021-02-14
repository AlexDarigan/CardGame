using System.Collections.Generic;

namespace CardGame.Server
{
    public class Player
    {
        public readonly int Id;
        public Player Opponent;
        public bool Ready = false;
        public readonly List<Card> Deck = new List<Card>();
        public readonly List<Card> Hand = new List<Card>();
        
        public Player(int id)
        {
            Id = id;
        }

        public void Draw()
        {
            Card card = Deck[Deck.Count - 1];
            Deck.Remove(card);
            Hand.Add(card);
        }
    }
}