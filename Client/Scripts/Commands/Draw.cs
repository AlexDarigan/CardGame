using System.Linq;

namespace CardGame.Client.Commands
{
    public class Draw : Command
    {
        private bool IsPlayer { get; }
        private int CardId { get; }

        public Draw(bool isPlayer, int cardId)
        {
            IsPlayer = isPlayer;
            CardId = cardId;
        }

        protected override void Setup(CommandQueue gfx)
        {
            Participant player = gfx.GetPlayer(IsPlayer);
            Card card = IsPlayer ? gfx.GetCard(CardId) : player.Deck.Last();
            MoveCard(card, player.Hand, gfx);
            UpdateZone(gfx, player.Hand);
        }
    }
}