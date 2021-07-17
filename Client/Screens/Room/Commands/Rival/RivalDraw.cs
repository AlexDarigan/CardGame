using System.Linq;
using System.Runtime.CompilerServices.Assets.Sounds;
using Godot;

namespace CardGame.Client.Commands
{
    public class RivalDraw: Command
    {
        protected override void Setup(Room room)
        {
            Participant rival = room.Rival;
            Card card = rival.Deck.Last();
            rival.Deck.Remove(card);
            rival.Hand.Add(card);
            
            const float duration = .2f;
            foreach (Card c in rival.Hand)
            {
                room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
			
            foreach (Card c in rival.Deck)
            {
                room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }

            room.Effects.InterpolateCallback(room.SFX, .1f, "set_stream", Sounds.Draw);
            room.Effects.InterpolateCallback(room.SFX, duration, "play");
        }
    }
}