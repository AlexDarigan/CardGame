using System.Linq;

namespace CardGame.Client.Commands
{
    public class RivalSetFaceDown: Command
    {
        protected override void Setup(Room room)
        {
            Participant rival = room.Rival;
            Card card = rival.Hand.Last();
            Move(card, rival.Supports);
        }
    }
}