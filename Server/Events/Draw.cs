using System;

namespace CardGame.Server.Events
{
    public class Draw : Event
    {
        private Card Card { get; }
        private Player Controller { get; }
      
        public Draw(Card card)
        {
            Controller = card.Controller;
            Card = card;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.PlayerDraw, Card.Id);
            queue(Controller.Opponent.Id, CommandId.RivalDraw);
        }
    }
}