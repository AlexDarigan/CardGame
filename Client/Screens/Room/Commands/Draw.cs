using System;
using System.Linq;

namespace CardGame.Client.Commands
{
    public class Draw : Command
    {
        private Who Who { get; }
        private int CardId { get; }
        
        public Draw(Who who, int cardId)
        {
            Who = who;
            CardId = cardId;
        }

        protected override void Setup(Room room)
        {
            Card card = Who == Who.Player ? room.Cards[CardId] : room.Rival.Deck.Last();
            Move(room, card, card.Controller.Hand);
        }
    }
}