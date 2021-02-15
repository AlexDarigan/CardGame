using System.Collections.Generic;

namespace CardGame.Server
{
    /*
     * Player is a Script that doesn't care about the game rules unlike Match
     * Largely that means what can happen here depends on who is invoking it (either
     * the game or a skill from a card). This allows us to test a number of actions with
     * ease in our Unit Tests
     */
    public class Player
    {
        public readonly int Id;
        public Player Opponent;
        public bool Ready = false;
        public readonly List<Card> Deck = new List<Card>();
        public readonly List<Card> Graveyard = new List<Card>();
        public readonly List<Card> Hand = new List<Card>();
        public readonly List<Card> Units = new List<Card>();
        public readonly List<Card> Supports = new List<Card>();

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