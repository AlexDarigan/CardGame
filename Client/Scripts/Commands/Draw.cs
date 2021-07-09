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

        protected override void Setup(Room room)
        {
            Participant player = room.GetPlayer(IsPlayer);
            Card card = IsPlayer ? room.GetCard(CardId) : player.Deck.Last();
            MoveCard(card, player.Hand, room);
            UpdateZone(room, player.Hand);
        }
    }
}