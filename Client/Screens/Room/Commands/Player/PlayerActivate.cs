using System.Runtime.CompilerServices.Assets.Sounds;
using Godot;

namespace CardGame.Client.Commands
{
    public class PlayerActivate: Command
    {
        private int CardId { get; init; }

        
        public PlayerActivate(int cardId)
        {
            CardId = cardId;
        }


        protected override void Setup(Room room)
        {
            Participant player = room.Player;
            Card card = room.Cards[CardId];

            Vector3 flipped = new Vector3(card.RotationDegrees.x, card.RotationDegrees.y, 0);
            room.Effects.InterpolateProperty(card, nameof(Card.RotationDegrees), card.RotationDegrees, flipped, 0.25f);
            room.Effects.InterpolateCallback(room.Link, .25f, nameof(room.Link.Activate), card);
            
            room.Effects.InterpolateCallback(room.SFX, .01f, "set_stream", Sounds.Activate);
            room.Effects.InterpolateCallback(room.SFX, .1f, "play");
        }
    }
}

