using System;
using System.Collections.Generic;
using System.Linq;
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
            foreach (Card card in Deck.Select(pair => createCard(pair.Key, pair.Value)))
            {
                Player.Deck.Add(card);
                card.Controller = Player;
                Location location = player.Deck.Locations.Last();
                card.Translation = location.Translation;
                card.RotationDegrees = location.RotationDegrees;
            }
        }

        protected override void Setup(CommandQueue gfx)
        {
            
        }
    }
}