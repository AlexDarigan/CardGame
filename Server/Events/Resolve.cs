using System;

namespace CardGame.Server.Events
{
    public class Resolve: Event
    {
        // This doesn't really matter but we do need to get the IDs somehow (maybe look into revising this?)
        private Player Player { get; }
        
        public Resolve(Player player)
        {
            Player = player;
            Command = CommandId.Resolve;
        }


        public override void QueueOnClients(Enqueue queue)
        {
            queue(Player.Id, Command);
            queue(Player.Opponent.Id, Command);
        }
    }
}