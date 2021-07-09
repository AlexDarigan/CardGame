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

        protected override void Setup(CommandQueue gfx)
        {
            Participant player = gfx.GetPlayer(IsPlayer);
            Card card = gfx.GetCard(CardId);
            MoveCard(player is Player ? card : player.Hand.Last(), player.Hand, player.Supports, gfx);
        }
    }
}