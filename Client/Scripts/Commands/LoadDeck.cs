using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class LoadDeck : Command
    {
        private Dictionary<int, SetCodes> Deck { get; }
        private bool IsPlayer { get; }

        public LoadDeck(bool isPlayer, Dictionary<int, SetCodes> deck, CommandQueue gfx)
        {
            IsPlayer = isPlayer;
            Deck = deck;

            // We execute this on instantiation because other commands will require the cards to exist to work
            // properly (however maybe we can investigate yielding constructors?)
            Participant player = gfx.GetPlayer(IsPlayer);
            foreach (Card card in Deck.Select(pair => gfx.GetCard(pair.Key, pair.Value)))
            {
                player.Deck.Add(card);
                card.Controller = player;
                Location location = player.Deck.Locations.Last();
                card.Translation = location.Translation;
                card.RotationDegrees = location.RotationDegrees;
            }
        }

        protected override void Setup(CommandQueue gfx) { }
    }
}