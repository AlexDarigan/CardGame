using System;
using Godot;

namespace CardGame.Client.Commands
{
    public class Activate: Command
    {
        private Who Who { get; }
        private int CardId { get; }
        private SetCodes SetCode { get; }
        private int Index { get; }
        
        public Activate(Who who, int cardId, SetCodes setCode, int index)
        {
            Who = who;
            CardId = cardId;
            SetCode = setCode;
            Index = index;
        }

        protected override void Setup(Room room)
        {
            Card card = room.Cards[CardId, SetCode];

            if (Who == Who.Rival)
            {
                Card fake = room.Rival.Supports[Index];
                room.Rival.Supports.Remove(fake);
                room.Rival.Supports.Insert(Index, card);

                card.Controller = room.Rival;
                card.Translation = fake.Translation;
                card.RotationDegrees = fake.RotationDegrees;
                fake.Free();
            }

            Vector3 flipped = new Vector3(card.RotationDegrees.x, card.RotationDegrees.y, 0);
            room.Effects.InterpolateProperty(card, nameof(Card.RotationDegrees), card.RotationDegrees, flipped, 0.25f);
            room.Effects.InterpolateCallback(room.Link, .25f, nameof(room.Link.Activate), card);
        }
    }
}
