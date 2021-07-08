using System;
using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class SetFaceDown : Command
    {
        private Participant Player { get; }
        private Card Card { get; }

        public SetFaceDown(Participant player, Card card)
        {
            Player = player;
            Card = card;
        }

        protected override void Setup(Tween gfx)
        {
            MoveCard(Player is Player ? Card : Player.Hand.Last(), Player.Hand, Player.Supports, gfx);
        }
    }
}