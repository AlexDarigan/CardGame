using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class Deploy : Command
    {
        public Deploy(bool isPlayer, int card, SetCodes setCodes)
        {
            IsPlayer = isPlayer;
            CardId = card;
            SetCode = setCodes;
        }

        protected override void Setup(Room room)
        {
            Participant player = room.GetPlayer(IsPlayer);
            Card card = room.GetCard(CardId, SetCode);
            if(!IsPlayer) { SwapFakeCardForRealCard(player, card);}
            MoveCard(card, player.Units, room);
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