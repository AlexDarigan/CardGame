using Godot;

namespace CardGame.Client.Commands
{
    public class SentToGraveyard: Command
    {
        
        public SentToGraveyard(int cardId)
        {
            CardId = cardId;
        }
        
        protected override void Setup(Room room)
        {
            Card card = room.GetCard(CardId);
            MoveCard(card, card.Controller.Discard, room);
        }
    }
}