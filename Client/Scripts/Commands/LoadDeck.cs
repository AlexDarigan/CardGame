using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
	public class LoadDeck : Command
	{
		private readonly Participant _player;
		private readonly Dictionary<int, SetCodes> _deck;

		public LoadDeck(Participant player, Dictionary<int, SetCodes> deck, Func<int, SetCodes, Card> createCard)
		{
			_player = player;
			_deck = deck;
		
			// We execute this on instantiation because other commands will require the cards to exist to work
			// properly (however maybe we can investigate yielding constructors?)
			foreach (KeyValuePair<int, SetCodes> pair in deck) { _player.Deck.Add(createCard(pair.Key, pair.Value)); }
		}
		
		protected override void Setup(Tween gfx) { }
	}
}
