namespace CardGame.Server
{
    public class SetEvent: Event
    {
        private readonly Player Controller;
        private readonly Card Card;

        public SetEvent(Player controller, Card card)
        {
            Command = CommandId.SetFaceDown;
            Controller = controller;
            Card = card;
        }
        
        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, Command, isClient, Card.Id);
            queue(Controller.Opponent.Id, Command, !isClient, -1);
        }
    }
}