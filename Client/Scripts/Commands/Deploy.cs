using System;
using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class Deploy : Command
    {
        private Participant Player { get; }
        private Card Card { get; }

        public Deploy(Participant player, Card card)
        {
            Player = player;
            Card = card;
        }

        protected override void Setup(Tween gfx)
        {
            SwapFakeCardForRealCard();
            Player.Hand.Remove(Card);
            Player.Units.Add(Card);
            
            const float duration = .2f;
            Location destination = Player.Units.Destination;
            
            // Shift Right
            foreach (Location location in Player.Hand.Locations)
            {
                gfx.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
            
            // Shift Left
            foreach (Location location in Player.Units.Locations)
            {
                gfx.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
            
            gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), Card.RotationDegrees,
                destination.RotationDegrees, duration);
            
        }

        private void SwapFakeCardForRealCard()
        {
            if (Player.IsClient) { return; }
            Card fake = Player.Hand.Last();
            Player.Hand.Remove(fake);
            Player.Hand.Add(Card);
            fake.Free();
            Card.Controller = Player;
        }
    }
}