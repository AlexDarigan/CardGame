using Godot;

namespace CardGame.Client.Commands
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
            
            foreach (Location location in Card.Controller.Units.Locations)
            {
                gfx.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
            
            gfx.InterpolateProperty(Card, nameof(Card.Translation), Card.Translation, destination.Translation,
                duration);
            
            gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), Card.RotationDegrees,
                destination.RotationDegrees, duration);
        }
    }
}