using System.Linq;
using Godot;

namespace CardGame.Client
{
    public class Draw: Command
    {
        private readonly Participant _player;
        private readonly Card Card;

        public Draw(Participant player, Card card)
        {
            _player = player;
            Card = card;
        }
		
        protected override void Setup(Tween gfx)
        {
            // Our rival doesn't have a real card, so we need to make a local check lest we end up moving the same card around 
            Card card = _player.IsClient ? Card : _player.Deck.Last();
            _player.Deck.Remove(card);
            _player.Hand.Add(card);
            Location destination = _player.Hand.Destination;
            const float duration = .35f;
            gfx.InterpolateProperty(card, nameof(Card.Translation), card.Translation, destination.Translation, duration, Tween.TransitionType.Linear,Tween.EaseType.In);
            gfx.InterpolateProperty(card, nameof(Card.RotationDegrees), card.RotationDegrees, destination.RotationDegrees, duration);

        }
    }
}