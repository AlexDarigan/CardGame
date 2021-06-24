using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
    public class LoadDeck : Command
    {
        private readonly Player _player;
        private readonly Dictionary<int, SetCodes> _deck;

        public LoadDeck(Player player, Dictionary<int, SetCodes> deck, Func<int, SetCodes, Card> createCard)
        {
            _player = player;
            _deck = deck;
		
            // We execute this on instantiation because other commands will require the cards to exist to work
            // properly (however maybe we can investigate yielding constructors?)
            foreach (KeyValuePair<int, SetCodes> pair in deck) { _player.Deck.Add(createCard(pair.Key, pair.Value)); }
        }
		
        public override void Execute(Tween gfx) { }
    }
}