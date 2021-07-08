using Godot;

namespace CardGame.Client.Commands
{
    public class SentToGraveyard: Command
    {
        private Card Card { get; }
        
        public SentToGraveyard(Card card)
        {
            Card = card;
        }
        
        protected override void Setup(Tween gfx)
        {
            MoveCard(Card, Card.Controller.Units, Card.Controller.Discard, gfx);
        }
    }
}