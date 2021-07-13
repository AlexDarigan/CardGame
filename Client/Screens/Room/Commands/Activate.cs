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
            // Card is created behind the scenes
            // We need to replace the fake card with the real card
            // We can set its controller to opponent right now
            Card fake = room.Rival.Supports[Index]; // Add an index value for more accurate activation?
            Card.Controller = room.Rival;
            Card.Translation = fake.Translation;
            Card.RotationDegrees = fake.RotationDegrees;
            
            // Currently having some problems with this but it should be resolved once we introduce passing play
            room.Rival.Supports.Remove(fake);
            room.Rival.Supports.Insert(Index, Card);
            
            Vector3 flipped = new Vector3(Card.RotationDegrees.x, Card.RotationDegrees.y, 0);
            room.Effects.InterpolateProperty(Card, nameof(Card.RotationDegrees), Card.RotationDegrees, flipped, .25f);
        }
    }
}

// protected override void Setup(Room room)
// {
//     if(Who == Who.Rival) { SwapFakeCardForRealCard();}
//     Card.Move(room, Player.Units);
// }
//
// private void SwapFakeCardForRealCard()
// {
//     Card fake = Player.Hand.Last();
//     Player.Hand.Remove(fake);
//     Player.Hand.Add(Card);
//     fake.Free();
//     Card.Controller = Player;
// }