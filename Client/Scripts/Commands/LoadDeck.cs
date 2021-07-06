using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
    public class LoadDeck : Command
    {
        private readonly Dictionary<int, SetCodes> _deck;
        private readonly Participant _player;

        public LoadDeck(Participant player, Dictionary<int, SetCodes> deck, Func<int, SetCodes, Card> createCard)
        {
            _player = player;
            _deck = deck;

            // We execute this on instantiation because other commands will require the cards to exist to work
            // properly (however maybe we can investigate yielding constructors?)
            foreach (KeyValuePair<int, SetCodes> pair in deck)
            {
                Card card = createCard(pair.Key, pair.Value);
                _player.Deck.Add(card);
                card.Controller = _player;
                Location destination = _player.Deck.Destination;
                card.Translation = destination.Translation;
                card.RotationDegrees = destination.RotationDegrees;
            }
        }

        protected override void Setup(Tween gfx)
        {
        }
    }
}