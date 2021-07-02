﻿using System.Collections.Generic;
using System.Linq;

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
        public readonly Zone Deck = new();
        public readonly IEnumerable<SetCodes> DeckList;
        public readonly Zone Graveyard = new();
        public readonly Zone Hand = new();
        public readonly int Id;
        public readonly Zone Supports = new();
        public readonly Zone Units = new();
        public bool Disqualified = false;
        public int Health = 8000;
        public Player Opponent;
        public bool Ready = false;
        public States State = States.Passive;

        public Player(int id, IEnumerable<SetCodes> deckList)
        {
            Id = id;
            DeckList = deckList;
        }

        public Event LoadDeck(CardRegister cardRegister)
        {
            foreach (SetCodes setCode in DeckList)
            {
                Card card = Library.Create(this, cardRegister, setCode);
                Deck.Add(card);
            }

            return new LoadDeckEvent(this, Deck.ToDictionary(card => card.Id, card => card.SetCodes));
        }

        public Event Draw()
        {
            Card card = Deck[Deck.Count - 1];
            Deck.Remove(card);
            Hand.Add(card);
            return new DrawEvent(card);
        }

        public Event Deploy(Card unit)
        {
            Hand.Remove(unit);
            Units.Add(unit);
            unit.Zone = Units;
            return new DeployEvent(this, unit);
        }

        public Event SetFaceDown(Card support)
        {
            Hand.Remove(support);
            Supports.Add(support);
            support.Zone = Supports;
            return new SetEvent(this, support);
        }
    }
}