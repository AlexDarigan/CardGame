namespace CardGame.Server.Events
{
    public class Activation: Event
    {
        private Card Activated { get; }
        
        public Activation(Card activated)
        {
            Activated = activated;
        }
        
        public override void QueueOnClients(Enqueue queue)
        {
            queue(Activated.Controller.Id, CommandId.Activate, Activated.Id);
            queue(Activated.Controller.Opponent.Id, CommandId.Activate, Activated.Id);
        }
    }
}