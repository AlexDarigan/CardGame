namespace CardGame.Server.Events
{
    public class DirectAttack: Event
    {
        private Card Attacker { get; }
        
        public DirectAttack(Card attacker) { Attacker = attacker; }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Attacker.Controller.Id, CommandId.DirectAttack, Attacker.Id);
            queue(Attacker.Controller.Opponent.Id, CommandId.DirectAttack, Attacker.Id);
        }
    }
}