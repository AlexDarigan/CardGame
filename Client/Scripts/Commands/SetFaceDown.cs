using System;
using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class SetFaceDown : Command
    {
        private bool PlayerId { get; }
        private int CardId { get; }

        public SetFaceDown(bool player, int card)
        {
            PlayerId = player;
            CardId = card;
        }

        protected override void Setup(CommandQueue gfx)
        {
            Participant player = gfx.GetPlayer(PlayerId);
            Card card = gfx.GetCard(CardId);
            MoveCard(player is Player ? card : player.Hand.Last(), player.Hand, player.Supports, gfx);
        }
    }
}