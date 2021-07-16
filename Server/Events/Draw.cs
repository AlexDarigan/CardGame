using System;

namespace CardGame.Server.Events
{
    public class Draw : Event
    {
        private Card Card { get; }
        private Player Controller { get; }
        private int Source { get; }
        private int Destination { get; }
        
        public Draw(Card card, int index, int destination)
        {
            Controller = card.Controller;
            Card = card;
            Source = index;
            Destination = destination;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.PlayerDraw, Card.Id);
            queue(Controller.Opponent.Id, CommandId.RivalDraw);
        }
    }
}