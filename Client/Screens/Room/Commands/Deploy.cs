using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class Deploy : Command
    {
        private Who Who { get; }
        private int CardId { get; }
        private SetCodes SetCode { get; }
        
        public Deploy(Who who, int card, SetCodes setCodes)
        {
            Who = who;
            CardId = card;
            SetCode = setCodes;
        }

        protected override void Setup(Room room)
        {
            Participant player = Who == Who.Player ? room.Player : room.Rival;
            Card card = room.Cards[CardId, SetCode];
            if(Who == Who.Rival) { SwapFakeCardForRealCard(player, card);} // Replace with clone methods
            Move(room, card, player.Units);
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