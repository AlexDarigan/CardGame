using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class Deploy : Command
    {
        private Participant Player { get; }
        private Card Card { get; }

        public Deploy(Participant player, Card card)
        {
            Player = player;
            Card = card;
        }

        protected override void Setup(Tween gfx)
        {
            SwapFakeCardForRealCard();
            MoveCard(Card, Player.Hand, Player.Units, gfx);
        }

        private void SwapFakeCardForRealCard()
        {
            if (Player is Player) { return; }
            Card fake = Player.Hand.Last();
            Player.Hand.Remove(fake);
            Player.Hand.Add(Card);
            fake.Free();
            Card.Controller = Player;
        }
    }
}