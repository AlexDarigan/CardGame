using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class RivalDeploy: Command
    {
        private int CardId { get; }
        private SetCodes SetCode { get; }
        
        public RivalDeploy(int cardId, SetCodes setCode)
        {
            CardId = cardId;
            SetCode = setCode;
        }
        
        protected override void Setup(Room room)
        {
            Participant rival = room.Rival;
            Card card = room.Cards[CardId, SetCode];
            card.OwningParticipant = rival;
            card.Controller = rival;
            // Replace a hidden card with our concrete one
            // For now we'll remove from the end of the hand (we'll worry about index later)
            Card fake = rival.Hand.Last();
            rival.Hand.Remove(fake);
            fake.Free();
            rival.Hand.Add(card);

            Zone origin = card.CurrentZone;
            origin.Remove(card);
            rival.Units.Add(card);

            const float duration = .2f;
            foreach (Card c in origin)
            {
                Room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                Room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
			
            foreach (Card c in rival.Units)
            {
                Room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                Room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
        }
    }
}