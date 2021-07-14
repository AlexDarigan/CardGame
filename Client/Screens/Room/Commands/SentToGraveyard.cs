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
            Move(room, Card, Card.Controller.Discard);
        }
    }
}