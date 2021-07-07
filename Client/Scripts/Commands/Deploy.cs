using System.Linq;
using Godot;

namespace CardGame.Client.Commands
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

        protected override void Setup(Tween gfx)
        {
            SwapFakeCardForRealCard();
            _player.Hand.Remove(Card);
            _player.Units.Add(Card);
            Location destination = _player.Units.Destination;
            const float duration = .35f;
            gfx.InterpolateProperty(Card, nameof(Card.Translation), Card.Translation, destination.Translation,
                duration);
            gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), Card.RotationDegrees,
                destination.RotationDegrees, duration);
            SortHand(gfx, _player);
        }

        private void SwapFakeCardForRealCard()
        {
            if (_player.IsClient) { return; }
            
            Card fake = _player.Hand.Last();
            _player.Hand.Remove(fake);
            _player.Hand.Add(Card);
            Card.Controller = _player;
        }
    }
}