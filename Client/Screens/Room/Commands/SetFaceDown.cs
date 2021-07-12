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
            if (Who == Who.Rival) { Card = Player.Hand.Last();}
            Card.Move(room, Player.Supports);
        }
    }
}