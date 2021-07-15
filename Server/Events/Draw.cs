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
           // Command = CommandId.Draw;
            Controller = card.Controller;
            Card = card;
            Source = index;
            Destination = destination;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.MoveCard, Who.Player, Card.Id, Card.SetCodes, Zones.Deck, Zones.Hand, Source, Destination);
            queue(Controller.Opponent.Id, CommandId.MoveCard, Who.Rival, 0, SetCodes.NullCard, Zones.Deck, Zones.Hand, Source, Destination);
        }
    }
}