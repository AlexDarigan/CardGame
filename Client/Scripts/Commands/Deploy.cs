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
            Location destination = Player.Units.Destination;
            const float duration = .35f;
            gfx.InterpolateProperty(Card, nameof(Card.Translation), Card.Translation, destination.Translation,
                duration);
            gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), Card.RotationDegrees,
                destination.RotationDegrees, duration);
            
            SortHand(gfx, Player);
        }

        private void SwapFakeCardForRealCard()
        {
            if (Player.IsClient) { return; }
            Card fake = Player.Hand[Player.Hand.Count - 1];
            Player.Hand.Remove(fake);
            Player.Hand.Add(Card);
            fake.Free();
            Card.Controller = Player;
        }
    }
}