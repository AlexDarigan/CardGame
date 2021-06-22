using System;

namespace CardGame.Server
{
    public class DrawEvent: Event
    {
        private readonly Player Controller;
        private readonly Card Card;
        
        public DrawEvent(Card card)
        {
            Command = CommandId.Draw;
            Controller = card.Controller;
            Card = card;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, Command, Controller.Id, Card.Id);
        }
    }
}
//private void Queue(int player, CommandId commandId, params object[] args)
//{
   // RpcId(player, "Queue", commandId, args);
//}
