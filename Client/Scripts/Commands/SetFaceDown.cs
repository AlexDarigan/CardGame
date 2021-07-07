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
            Player.Hand.Remove(Card);
            Player.Supports.Add(Card);
            Location destination = Player.Supports.Destination;
            const float duration = .35f;
            gfx.InterpolateProperty(card, nameof(Card.Translation), card.Translation, destination.Translation,
                duration);
            gfx.InterpolateProperty(card, nameof(Card.RotationDegrees), Card.RotationDegrees,
                destination.RotationDegrees, duration);
            SortHand(gfx, Player);
        }
    }
}