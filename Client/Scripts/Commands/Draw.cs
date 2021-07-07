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
            // Our rival doesn't have a real card, so we need to make a local check lest we end up moving the same card around 
            Card card = Player.IsClient ? Card : Player.Deck.Last();
            Player.Deck.Remove(card);
            Player.Hand.Add(card);
            Location destination = Player.Hand.Destination;
            const float duration = .35f;
            gfx.InterpolateProperty(card, nameof(Card.Translation), card.Translation, destination.Translation, duration,
                Tween.TransitionType.Linear, Tween.EaseType.In);
            gfx.InterpolateProperty(card, nameof(Card.RotationDegrees), card.RotationDegrees,
                destination.RotationDegrees, duration);
        }
    }
}