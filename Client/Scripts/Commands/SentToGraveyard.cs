using Godot;

namespace CardGame.Client
{
    public class SentToGraveyard: Command
    {
        private Card Card { get; }
        
        public SentToGraveyard(Card card)
        {
            Card = card;
        }
        
        protected override void Setup(Tween gfx)
        {
            Card.Controller.Units.Remove(Card);
            Card.Controller.Discard.Add(Card);

            Location destination = Card.Controller.Discard.Destination;
            const float duration = .35f;
            gfx.InterpolateProperty(Card, nameof(Card.Translation), Card.Translation, destination.Translation,
                duration);
            gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), Card.RotationDegrees,
                destination.RotationDegrees, duration);
        }
    }
}