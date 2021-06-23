using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client
{
    public class LoadDeck : Command
    {
        private readonly Player _player;
        private readonly Dictionary<int, SetCodes> _deck;

        public LoadDeck(Player player, Dictionary<int, SetCodes> deck, Register register)
        {
            _player = player;
            _deck = deck;
		
            // We execute this on instantiation because other commands will require the cards to exist to work
            // properly (however maybe we can investigate yielding constructors?)
            foreach (KeyValuePair<int, SetCodes> pair in deck)
            {
                register.Add(pair.Key, pair.Value);
                player.Deck.Add(register[pair.Key]);
            }
        }
		
        public override SignalAwaiter Execute(Tween gfx)
        { 
            CallDeferred("emit_signal", "NullCommand");
            return ToSignal(this, "NullCommand");
        }
    }
}