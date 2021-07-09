using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class Deploy : Command
    {
        private bool IsPlayer { get; }
        private int CardId { get; }
        private SetCodes SetCode { get; }
        
        public Deploy(bool isPlayer, int card, SetCodes setCodes)
        {
            IsPlayer = isPlayer;
            CardId = card;
            SetCode = setCodes;
        }

        protected override void Setup(CommandQueue gfx)
        {
            Participant player = gfx.GetPlayer(IsPlayer);
            Card card = gfx.GetCard(CardId, SetCode);
            if(!IsPlayer) { SwapFakeCardForRealCard(player, card);}
            MoveCard(card, player.Hand, player.Units, gfx);
        }

        private void SwapFakeCardForRealCard(Participant player, Card card)
        {
            Card fake = player.Hand.Last();
            player.Hand.Remove(fake);
            player.Hand.Add(card);
            fake.Free();
            card.Controller = player;
        }
    }
}