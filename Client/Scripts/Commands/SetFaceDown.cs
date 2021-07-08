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
            Card card = Player.IsClient ? Card : Player.Hand.Last();
            Player.Hand.Remove(card);
            Player.Supports.Add(card);
            Location destination = Player.Supports.Destination;
            const float duration = .2f;
            
            // Shift Right
            foreach (Location location in Player.Hand.Locations)
            {
                gfx.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
            
            // Shift Left
            foreach (Location location in Player.Supports.Locations)
            {
                gfx.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
            
            gfx.InterpolateProperty(card, nameof(Card.RotationDegrees), card.RotationDegrees,
                destination.RotationDegrees, duration);
        }
    }
}