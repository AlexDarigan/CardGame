namespace CardGame.Server.Events
{
    public class Battle: Event
    {
        private Card Attacker { get; }
        private Card Defender { get; }

        public Battle(Card attacker, Card defender)
        {
            Command = CommandId.Battle;
            Attacker = attacker;
            Defender = defender;
        }
        
        public override void QueueOnClients(Enqueue queue)
        {
            queue(Attacker.Controller.Id, Command, Attacker.Id, Defender.Id);
            queue(Defender.Controller.Id, Command, Attacker.Id, Defender.Id);
        }
    }
}