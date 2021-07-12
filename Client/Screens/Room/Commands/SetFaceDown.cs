using System;
using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class SetFaceDown : Command
    {
        private bool IsPlayer { get; }
        private int CardId { get; }

        public SetFaceDown(bool isPlayer, int card)
        {
            IsPlayer = isPlayer;
            CardId = card;
        }

        protected override void Setup(Room room)
        {
            Participant player = room.GetPlayer(IsPlayer);
            Card card = room.GetCard(CardId);
            MoveCard(player.IsClient ? card : player.Hand.Last(), player.Supports, room);
        }
    }
}