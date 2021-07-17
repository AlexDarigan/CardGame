using System.Runtime.CompilerServices.Assets.Sounds;
using Godot;

namespace CardGame.Client.Commands
{
    public class PlayerDeploy: Command
    {
        private int CardId { get; }

        public PlayerDeploy(int cardId)
        {
            CardId = cardId;
        }


        protected override void Setup(Room room)
        {
            Participant player = room.Player;
            Card card = room.Cards[CardId];
            Zone origin = card.CurrentZone;
            origin.Remove(card);
            player.Units.Add(card);

            const float duration = .2f;
            foreach (Card c in origin)
            {
                Room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                Room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
			
            foreach (Card c in player.Units)
            {
                Room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                Room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
            
            room.Effects.InterpolateCallback(room.SFX, .1f, "set_stream", Sounds.Deploy);
            room.Effects.InterpolateCallback(room.SFX, duration, "play");
            
        }
    }
}