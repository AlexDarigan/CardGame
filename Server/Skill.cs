using System.Collections.Generic;

namespace CardGame.Server
{
    public class Skill
    {
        public string Description { get; }
        public IEnumerable<int> OpCodes { get; }
        public Card Owner { get; }
        public IEnumerable<Triggers> Triggers { get; }

        public Skill(Card owner, IEnumerable<Triggers> triggers, IEnumerable<int> opCodes, string description)
        {
            Owner = owner;
            Triggers = triggers;
            OpCodes = opCodes;
            Description = description;
        }
    }
}