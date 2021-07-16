using System.ComponentModel.Design;

namespace CardGame.Server.Events
{
    public class AttackUnit: Event
    {
        private Card Attacker { get; }
        private Card Defender { get; }

        public AttackUnit(Card attacker, Card defender)
        {
            Attacker = attacker;
            Defender = defender;
        }
        
        public override void QueueOnClients(Enqueue queue)
        {
            queue(Attacker.Controller.Id, CommandId.AttackUnit, Attacker.Id, Defender.Id);
            queue(Defender.Controller.Id, CommandId.AttackUnit, Attacker.Id, Defender.Id);
        }
    }
}