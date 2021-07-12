using System;
using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class SetFaceDown : Command
    {
        public SetFaceDown(Who who, int card)
        {
            Who = who;
            CardId = card;
        }

        protected override void Setup(Room room)
        {
            MoveCard(Who == Who.Player ? Card : Player.Hand.Last(), Player.Supports, room);
        }
    }
}