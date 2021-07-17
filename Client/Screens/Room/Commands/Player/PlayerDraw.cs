using System.Runtime.CompilerServices.Assets.Sounds;
using Godot;

namespace CardGame.Client.Commands
{
    public class PlayerDraw: Command
    {
        private int CardId { get; }
        
        public PlayerDraw(int cardId)
        {
            CardId = cardId;
        }
        
        protected override void Setup(Room room)
        {
            Participant player = room.Player;
            Card card = room.Cards[CardId];
            
            // Remove + Add so our card's location is at the top
            player.Deck.Remove(card);
            player.Deck.Add(card);
            Zone origin = player.Deck;
            Zone destination = player.Hand;
            
            // Zone origin = card.CurrentZone;
            origin.Remove(card);
            destination.Add(card);
            
            const float duration = .2f;
            foreach (Card c in origin)
            {
                room.Effects.InterpolateProperty(c, nameof(Card.Translation), c.Translation, c.Location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                room.Effects.InterpolateProperty(c, nameof(Card.RotationDegrees), c.RotationDegrees, c.Location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
			
            foreach (Card c in destination)
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