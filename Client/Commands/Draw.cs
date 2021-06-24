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
		
        public override void Execute(Tween gfx)
        {
            // Our rival doesn't have a real card, so we need to make a local check lest we end up moving the same card around 
            Card card = _player.IsClient ? Card : _player.Deck.Last();
            Location source = _player.Deck.Remove(card);
            Location destination = _player.Hand.Add(card);
            _player.Deck.Top.Visible = false;
            card.Translation = source.Translation;
            card.RotationDegrees = source.RotationDegrees;
            const float duration = .35f;
            gfx.InterpolateProperty(card, Translation, card.Translation, destination.Translation, duration);
            gfx.InterpolateProperty(card, RotationDegrees, card.RotationDegrees, destination.RotationDegrees, duration);

        }
    }
}