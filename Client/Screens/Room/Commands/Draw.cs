using System;
using System.Linq;

namespace CardGame.Client.Commands
{
    public class Draw : Command
    {
        public Draw(Who who, int cardId)
        {
            Who = who;
            CardId = cardId;
        }

        protected override void Setup(Room room)
        {
            Card card = Who == Who.Player ? Card : Player.Deck.Last();
            MoveCard(card, Player.Hand, room);
            UpdateZone(room, Player.Hand);
        }
    }
}