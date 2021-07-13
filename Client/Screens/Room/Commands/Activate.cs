using System;
using Godot;

namespace CardGame.Client.Commands
{
    public class Activate: Command
    {
        private int Index { get; }
        
        public Activate(int cardId, SetCodes setCode, int index)
        {
            CardId = cardId;
            SetCode = setCode;
            Index = index;
        }

        protected override void Setup(Room room)
        {
            Card fake = room.Rival.Supports[Index];
            room.Rival.Supports.Remove(fake);
            room.Rival.Supports.Insert(Index, Card);
            
            Card.Controller = room.Rival;
            Card.Translation = fake.Translation;
            Card.RotationDegrees = fake.RotationDegrees;
            fake.Free();

            Vector3 flipped = new Vector3(Card.RotationDegrees.x, Card.RotationDegrees.y, 0);
            room.Effects.InterpolateProperty(Card, nameof(Card.RotationDegrees), Card.RotationDegrees, flipped, .25f);
        }
    }
}
