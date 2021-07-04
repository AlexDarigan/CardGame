using System.Collections.Generic;

namespace CardGame.Server
{
    public class Skill
    {
        public readonly string Description;
        public readonly IReadOnlyList<int> Instructions;
        public readonly Card Owner;
        public readonly IEnumerable<Triggers> Triggers;

        public Skill(Card owner, IEnumerable<Triggers> triggers, IReadOnlyList<int> instructions, string description)
        {
            Owner = owner;
            Triggers = triggers;
            Instructions = instructions;
            Description = description;
        }
    }
}