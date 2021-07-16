namespace CardGame.Server.Events
{
    public class AttackParticipant: Event
    {
        private Card Attacker { get; }
        
        public AttackParticipant(Card attacker) { Attacker = attacker; }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Attacker.Controller.Id, CommandId.AttackParticipant, Attacker.Id);
            queue(Attacker.Controller.Opponent.Id, CommandId.AttackParticipant, Attacker.Id);
        }
    }
}