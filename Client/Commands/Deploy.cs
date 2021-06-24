using System;
using System.Linq;
using Godot;

namespace CardGame.Client
{
    public class Deploy : Command
    {
        private readonly Participant _player;
        private readonly Card Card;

        public Deploy(Participant player, Card card)
        {
            _player = player;
            Card = card;
        }
		
        public override void Execute(Tween gfx)
        {
            Card card = _player.isClient ? Card : _player.Hand.Last();
            Location source = _player.Hand.Remove(Card);
            Location destination = _player.Units.Add(Card);
            const float duration = .35f;
            gfx.InterpolateProperty(card, Translation, source.Translation, destination.Translation,  duration);
            gfx.InterpolateProperty(card, RotationDegrees,  source.RotationDegrees, destination.RotationDegrees, duration);
        }
    }
}