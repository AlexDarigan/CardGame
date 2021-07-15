using System.Collections.Generic;

namespace CardGame.Server.Events
{
    public class EndTurn: Event
    {
        private Player Player { get; }

        public EndTurn(Player player)
        {
            Player = player;
        }
        
        
        public override void QueueOnClients(Enqueue queue)
        {
            queue(Player.Id, CommandId.EndTurn);
            queue(Player.Opponent.Id, CommandId.EndTurn);
        }
    }
}