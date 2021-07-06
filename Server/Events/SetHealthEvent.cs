namespace CardGame.Server
{
    public class SetHealthEvent: Event
    {
        private int NewHealth;
        private Player Damaged;

        public SetHealthEvent(Player damaged)
        {
            Command = CommandId.SetHealth;
            NewHealth = damaged.Health;
            Damaged = damaged;
        }
        
        public override void QueueOnClients(Enqueue queue)
        { 
            queue(Damaged.Id, Command, isClient, NewHealth);
            queue(Damaged.Opponent.Id, Command, !isClient, NewHealth);
        }
    }
}