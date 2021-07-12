namespace CardGame.Server.Events
{
    public class SetHealth: Event
    {
        private int NewHealth { get; }
        private Player Damaged { get; }

        public SetHealth(Player damaged)
        {
            Command = CommandId.SetHealth;
            NewHealth = damaged.Health;
            Damaged = damaged;
        }
        
        public override void QueueOnClients(Enqueue queue)
        { 
            queue(Damaged.Id, Command, Who.Player, NewHealth);
            queue(Damaged.Opponent.Id, Command, Who.Rival, NewHealth);
        }
    }
}