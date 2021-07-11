namespace CardGame.Server.Events
{
    public class GameOver: Event
    {
        private const bool IsLoser = true;
        private Player Loser;
        // Add Reason?

        public GameOver(Player loser) { Loser = loser; }
        
        public override void QueueOnClients(Enqueue queue)
        {
            queue(Loser.Id, CommandId.GameOver, IsLoser);
            queue(Loser.Opponent.Id, CommandId.GameOver, !IsLoser);
        }
    }
}