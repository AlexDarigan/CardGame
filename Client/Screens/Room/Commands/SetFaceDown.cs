using System;
using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class SetFaceDown : Command
    {
        
        private Who Who { get; }
        private int CardId { get; }
        
        public SetFaceDown(Who who, int card)
        {
            Who = who;
            CardId = card;
        }

        protected override void Setup(Room room)
        {
            Participant player = Who == Who.Player? room.Player: room.Rival;
            Card card = Who == Who.Player ? room.Cards[CardId] : room.Rival.Hand.Last();
            Move(room, card, player.Supports);
        }
    }
}