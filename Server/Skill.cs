using System;
using System.Collections.Generic;

namespace CardGame.Server
{
    public class Skill
    {
        public readonly Card Owner;
        public readonly IEnumerable<Triggers> Triggers;
        public readonly IEnumerable<Instructions> Instructions;
        public readonly string Description;
        public IList<object> Arguments;

        public Skill(Card owner, IEnumerable<Triggers> triggers, IEnumerable<Instructions> instructions, string desc)
        {
            Owner = owner;
            Triggers = triggers;
            Instructions = instructions;
            Description = desc;
        }
    }
}