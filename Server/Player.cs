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
        public enum States { Idle, Passive, Loser, Winner }
        public readonly int Id;
        public Player Opponent;
        public bool Ready = false;
        public readonly IEnumerable<SetCodes> DeckList;
        public readonly List<Card> Deck = new List<Card>();
        public readonly List<Card> Graveyard = new List<Card>();
        public readonly List<Card> Hand = new List<Card>();
        public readonly List<Card> Units = new List<Card>();
        public readonly List<Card> Supports = new List<Card>();
        public States State = States.Passive;
        public bool Disqualified = false;
        public int Health = 8000;

        public Player(int id, IEnumerable<SetCodes> deckList)
        {
            Id = id;
            DeckList = deckList;
        }

        public void LoadDeck(CardRegister cardRegister)
        {
            foreach (SetCodes setCode in DeckList)
            {
                Card card = Library.Create(this, cardRegister, setCode);
                Deck.Add(card);
            }
        }

        public DrawEvent Draw()
        {
            Card card = Deck[Deck.Count - 1];
            Deck.Remove(card);
            Hand.Add(card);
            return new DrawEvent(card);
        }

        public void Deploy(Card unit)
        {
            Hand.Remove(unit);
            Units.Add(unit);
            unit.Zone = Units;
        }

        public void SetFaceDown(Card support)
        {
            Hand.Remove(support);
            Supports.Add(support);
        }

    }
}