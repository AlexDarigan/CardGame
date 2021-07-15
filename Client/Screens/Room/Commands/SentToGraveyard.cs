using Godot;

namespace CardGame.Client.Commands
{
    public class SentToGraveyard: Command
    {
        private int CardId { get; }
        
        public SentToGraveyard(int cardId)
        {
            CardId = cardId;
        }
        
        protected override void Setup(Room room)
        {
            Card card = room.Cards[CardId];
            Move(room, card, card.Controller.Graveyard);
        }
    }
}