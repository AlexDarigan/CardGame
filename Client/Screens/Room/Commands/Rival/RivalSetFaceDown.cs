using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class RivalSetFaceDown: Command
    {
        protected override void Setup(Room room)
        {
            Participant rival = room.Rival;
            Card card = rival.Hand.Last();
            Zone origin = card.CurrentZone;
            origin.Remove(card);
            rival.Supports.Add(card);

            const float duration = .2f;
            foreach (Card c in origin)
            {
                Room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                Room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
			
            foreach (Card c in rival.Supports)
            {
                Room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                Room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
        }
    }
}