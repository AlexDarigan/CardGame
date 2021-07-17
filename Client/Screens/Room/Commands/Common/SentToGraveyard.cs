using Godot;

namespace CardGame.Client.Commands
{
    public class SentToGraveyard: Command
    {
        private int CardId { get; }
        
        public SentToGraveyard(int cardId)
        {
            CardId = cardId;
        }
        
        protected override void Setup(Room room)
        {
            Card card = room.Cards[CardId];
            Zone origin = card.CurrentZone;
            origin.Remove(card);
            card.OwningParticipant.Discard.Add(card);

            const float duration = .2f;
            foreach (Card c in origin)
            {
                Room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                Room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
			
            foreach (Card c in card.OwningParticipant.Discard)
            {
                Room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                Room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
        }
    }
}