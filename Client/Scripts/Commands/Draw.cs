using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class Draw : Command
    {
        private Participant Player { get; }
        private Card Card { get; }

        public Draw(Participant player, Card card)
        {
            Player = player;
            Card = card;
        }

        protected override void Setup(Tween gfx)
        {
            MoveCard(Player is Player ? Card : Player.Deck.Last(), Player.Deck, Player.Hand, gfx);
        }
    }
}