using CardGame.Server.Events;

namespace CardGame.Server.Bytecode
{
    public static partial class VirtualMachine
    {
        private static void SetHealth(SkillState skill)
        {
            Player player = GetPlayer(skill);
            int newHealth = skill.PopBack();
            player.Health = newHealth;
        }
        private static void SetFaction(SkillState skill) { }
        private static void SetPower(SkillState skill) { }
        private static void Destroy(SkillState skill) { }
        private static void DealDamage(SkillState skill) { }
        
        private static void Draw(SkillState skill)
        {
            Player player = GetPlayer(skill);
            int count = skill.PopBack();
            for (int i = 0; i < count; i++)
            {
                Card card = player.Deck[player.Deck.Count - 1];
                player.Deck.Remove(card);
                player.Hand.Add(card);
                skill.AddEvent(new Draw(card));
            }
        }
    }
}