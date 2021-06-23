using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
    public class LoadDeck : Command
    {
        private readonly Player _player;
        private readonly Dictionary<int, SetCodes> _deck;

        public LoadDeck(Player player, Dictionary<int, SetCodes> deck, Action<int, SetCodes> createCard, IReadOnlyDictionary<int, Card> cards)
        {
            _player = player;
            _deck = deck;
		
            // We execute this on instantiation because other commands will require the cards to exist to work
            // properly (however maybe we can investigate yielding constructors?)
            foreach (KeyValuePair<int, SetCodes> pair in deck)
            {
                createCard(pair.Key, pair.Value);
                player.Deck.Add(cards[pair.Key]);
            }
        }
		
        public override void Execute(Tween gfx) { }
    }
}