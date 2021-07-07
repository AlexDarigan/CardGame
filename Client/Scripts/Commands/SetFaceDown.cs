using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class SetFaceDown : Command
    {
        private readonly Participant _player;
        private readonly Card Card;

        public SetFaceDown(Participant player, Card card)
        {
            _player = player;
            Card = card;
        }

        protected override void Setup(Tween gfx)
        {
            Card card = _player.IsClient ? Card : _player.Hand.Last();
            _player.Hand.Remove(Card);
            _player.Supports.Add(Card);
            Location destination = _player.Supports.Destination;
            const float duration = .35f;
            gfx.InterpolateProperty(card, nameof(Card.Translation), card.Translation, destination.Translation,
                duration);
            gfx.InterpolateProperty(card, nameof(Card.RotationDegrees), Card.RotationDegrees,
                destination.RotationDegrees, duration);
            SortHand(gfx, _player);
        }
    }
}