using System.Linq;

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
            int index = Activated.Controller.Supports.FindIndex(Activated);
            queue(Activated.Controller.Opponent.Id, CommandId.Activate, Activated.Id, Activated.SetCodes, index);
        }
    }
}