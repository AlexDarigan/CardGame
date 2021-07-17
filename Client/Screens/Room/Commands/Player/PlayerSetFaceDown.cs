using System.Runtime.CompilerServices.Assets.Sounds;
using Godot;

namespace CardGame.Client.Commands
{
    public class PlayerSetFaceDown: Command
    {
        private int CardId { get; }

        public PlayerSetFaceDown(int cardId)
        {
            CardId = cardId;
        }

        protected override void Setup(Room room)
        {
            Participant player = room.Player;
            Card card = room.Cards[CardId];
            Zone origin = card.CurrentZone;
            origin.Remove(card);
            player.Supports.Add(card);

            const float duration = .2f;
            foreach (Card c in origin)
            {
                Room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                Room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
			
            foreach (Card c in player.Supports)
            {
                Room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                Room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
            
            room.Effects.InterpolateCallback(room.SFX, .0f, "set_stream", Sounds.SetFaceDown);
            room.Effects.InterpolateCallback(room.SFX, .05f, "play");
        }
    }
}