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
            queue(Player.Id, CommandId.PlayerActivate,Activated.Id);
            queue(Player.Opponent.Id, CommandId.RivalActivate,Activated.Id, Activated.SetCodes);
        }
    }
}