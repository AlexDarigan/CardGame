using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server
{
    public class Skill
    {
        public readonly Card Owner;
        public readonly IEnumerable<Triggers> Triggers;
        public readonly IEnumerable<int> Instructions;
        public readonly string Description;

        public Skill(Card owner, IEnumerable<Triggers> triggers, IEnumerable<int> instructions, string description)
        {
            Owner = owner;
            Triggers = triggers;
            Instructions = instructions;
            Description = description;
        }
    }
}