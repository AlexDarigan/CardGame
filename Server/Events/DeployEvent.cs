namespace CardGame.Server
{
    public class DeployEvent: Event
    {
        private readonly Player Controller;
        private readonly Card Card;

        public DeployEvent(Player controller, Card card)
        {
            Command = CommandId.Deploy;
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