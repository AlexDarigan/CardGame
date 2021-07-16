using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class RivalActivate: Command
    {
        private int CardId { get; }
        private SetCodes SetCode { get; }
        
        public RivalActivate(int cardId, SetCodes setCode)
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
            Card fake = rival.Supports.Last();
            rival.Supports.Remove(fake);
            rival.Supports.Add(card);
            card.Translation = fake.Translation;
            card.RotationDegrees = fake.RotationDegrees;
            fake.Free();
            
            Vector3 flipped = new Vector3(card.RotationDegrees.x, card.RotationDegrees.y, 0);
            room.Effects.InterpolateProperty(card, nameof(Card.RotationDegrees), card.RotationDegrees, flipped, 0.25f);
            room.Effects.InterpolateCallback(room.Link, .25f, nameof(room.Link.Activate), card);
            
        }
    }
}