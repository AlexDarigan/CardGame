using System.Linq;
using Godot;

namespace CardGame.Client.Commands
{
    public class Deploy : Command
    {
        public Deploy(Who who, int card, SetCodes setCodes)
        {
            Who = who;
            CardId = card;
            SetCode = setCodes;
        }

        protected override void Setup(Room room)
        {
            if(Who == Who.Rival) { SwapFakeCardForRealCard();}
            Card.Move(room, Player.Units);
        }

        private void SwapFakeCardForRealCard()
        {
            Card fake = Player.Hand.Last();
            Player.Hand.Remove(fake);
            Player.Hand.Add(Card);
            fake.Free();
            Card.Controller = Player;
        }
    }
}