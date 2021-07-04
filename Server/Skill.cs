using System.Collections.Generic;

namespace CardGame.Server
{
    public class Skill
    {
        public string Description { get; }
        public IReadOnlyList<int> Instructions { get; }
        public Card Owner { get; }
        public IEnumerable<Triggers> Triggers { get; }

        public Skill(Card owner, IEnumerable<Triggers> triggers, IReadOnlyList<int> instructions, string description)
        {
            Owner = owner;
            Triggers = triggers;
            Instructions = instructions;
            Description = description;
        }
    }
}