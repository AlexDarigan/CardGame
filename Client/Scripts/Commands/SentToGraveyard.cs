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
        
        protected override void Setup(CommandQueue gfx)
        {
            Card card = gfx.GetCard(CardId);
            MoveCard(card, card.Controller.Units, card.Controller.Discard, gfx);
        }
    }
}