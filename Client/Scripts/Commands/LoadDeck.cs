using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client.Commands
{
    public class LoadDeck : Command
    {
        private Dictionary<int, SetCodes> Deck { get; }
        private Participant Player { get; }

        public LoadDeck(Participant player, Dictionary<int, SetCodes> deck, Func<int, SetCodes, Card> createCard)
        {
            Player = player;
            Deck = deck;

            // We execute this on instantiation because other commands will require the cards to exist to work
            // properly (however maybe we can investigate yielding constructors?)
            foreach (KeyValuePair<int, SetCodes> pair in Deck)
            {
                Card card = createCard(pair.Key, pair.Value);
                Player.Deck.Add(card);
                card.Controller = Player;
                Location destination = Player.Deck.Destination;
                card.Translation = destination.Translation;
                card.RotationDegrees = destination.RotationDegrees;
            }
        }

        protected override void Setup(Tween gfx)
        {
        }
    }
}