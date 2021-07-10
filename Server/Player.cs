using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Events;

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
        public Zone Deck { get; } = new();
        private IEnumerable<SetCodes> DeckList { get; }
        public Zone Graveyard { get; } = new();
        public Zone Hand { get; } = new();
        public int Id;
        public Zone Supports { get; } = new();
        public Zone Units { get; } = new();
        public Illegal ReasonPlayerWasDisqualified = Illegal.NotDisqualified;
        
        // TODO: Remove this when old tests are removed
        public const bool Disqualified = false;
        public int Health = 8000;
        public Player Opponent;
        public bool Ready = false;
        public States State { get; set; } = States.Passive;

        public Player(int id, IEnumerable<SetCodes> deckList)
        {
            Id = id;
            DeckList = deckList;
        }

        public Event LoadDeck(Cards cards)
        {
            foreach (SetCodes setCode in DeckList)
            {
                Card card = cards.CreateCard(setCode, this);
                Deck.Add(card);
            }

            return new LoadDeck(this, Deck.ToDictionary(card => card.Id, card => card.SetCodes));
        }

        public Event Draw()
        {
            Card card = Deck[Deck.Count - 1];
            Deck.Remove(card);
            Hand.Add(card);
            return new Draw(card);
        }

        public Event Deploy(Card unit)
        {
            Hand.Remove(unit);
            Units.Add(unit);
            unit.Zone = Units;
            return new Deploy(this, unit);
        }

        public Event SetFaceDown(Card support)
        {
            Hand.Remove(support);
            Supports.Add(support);
            support.Zone = Supports;
            return new SetFaceDown(this, support);
        }

        public Event EndTurn()
        {
            return new EndTurn(this);
        }
    }
}