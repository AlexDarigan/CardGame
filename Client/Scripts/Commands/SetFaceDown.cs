using System;
using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class SetFaceDown : Command
    {
        private Participant Player { get; }
        private Card Card { get; }

        public SetFaceDown(Participant player, Card card)
        {
            Player = player;
            Card = card;
        }

        protected override void Setup(Tween gfx)
        {
            Card card = Player is Player ? Card : Player.Hand.Last();
            Player.Hand.Remove(card);
            Player.Supports.Add(card);
            Location destination = Player.Supports.Destination;
            const float duration = .2f;
            UpdateZone(gfx, Player.Hand);
            UpdateZone(gfx, Player.Supports);
            
            gfx.InterpolateProperty(card, nameof(Card.RotationDegrees), card.RotationDegrees,
                destination.RotationDegrees, duration);
        }
    }
}