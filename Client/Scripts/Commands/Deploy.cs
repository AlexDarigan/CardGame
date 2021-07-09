using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class Deploy : Command
    {
        private bool PlayerId { get; }
        private int CardId { get; }
        private SetCodes SetCode { get; }
        
        public Deploy(bool player, int card, SetCodes setCodes)
        {
            PlayerId = player;
            CardId = card;
            SetCode = setCodes;
        }

        protected override void Setup(CommandQueue gfx)
        {
            Participant player = gfx.GetPlayer(PlayerId);
            Card card = gfx.GetCard(CardId, SetCode);
            SwapFakeCardForRealCard(player, card);
            MoveCard(card, player.Hand, player.Units, gfx);
        }

        private void SwapFakeCardForRealCard(Participant player, Card card)
        {
            if (player is Player) { return; }
            Card fake = player.Hand.Last();
            player.Hand.Remove(fake);
            player.Hand.Add(card);
            fake.Free();
            card.Controller = player;
        }
    }
}