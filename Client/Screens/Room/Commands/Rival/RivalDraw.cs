using System.Linq;

namespace CardGame.Client.Commands
{
    public class RivalDraw: Command
    {
        protected override void Setup(Room room)
        {
            Participant rival = room.Rival;
            Card card = rival.Deck.Last();
            rival.Deck.Remove(card);
            rival.Hand.Add(card);
            Move(card, rival.Hand);
        }
    }
}