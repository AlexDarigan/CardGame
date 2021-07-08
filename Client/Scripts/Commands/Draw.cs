using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class Draw : Command
    {
        private Participant Player { get; }
        private Card Card { get; }

        public Draw(Participant player, Card card)
        {
            Player = player;
            Card = card;
        }

        protected override void Setup(Tween gfx)
        {
            Card card = Player.IsClient ? Card : Player.Deck.Last();
            Player.Deck.Remove(card);
            Player.Hand.Add(card);
            const float duration = .2f;
            Location destination = Player.Hand.Destination;
            UpdateZone(gfx, Player.Hand);
                       
            gfx.InterpolateProperty(card, nameof(Card.RotationDegrees), Card.RotationDegrees,
            destination.RotationDegrees, duration);

        }
    }
}