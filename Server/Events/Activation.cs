using System.Linq;

namespace CardGame.Server.Events
{
    public class Activation: Event
    {
        private Player Player { get; }
        private Card Activated { get; }
        
        public Activation(Player player, Card activated)
        {
            Player = player;
            Activated = activated;
        }
        
        public override void QueueOnClients(Enqueue queue)
        {
            int index = Activated.Controller.Supports.FindIndex(Activated);
            queue(Activated.Controller.Id, CommandId.Activate, Who.Player, Activated.Id, Activated.SetCodes, index);
            queue(Activated.Controller.Opponent.Id, CommandId.Activate, Who.Rival, Activated.Id, Activated.SetCodes, index);
        }
    }
}