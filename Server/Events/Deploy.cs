namespace CardGame.Server.Events
{
    public class Deploy : Event
    {
        private readonly Card Card;
        private readonly Player Controller;

        public Deploy(Player controller, Card card)
        {
            Command = CommandId.Deploy;
            Controller = controller;
            Card = card;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, Command, isClient, Card.Id, Card.SetCodes);
            queue(Controller.Opponent.Id, Command, !isClient, Card.Id, Card.SetCodes);
        }
    }
}