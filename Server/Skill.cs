using System.Collections.Generic;

namespace CardGame.Server
{
    public class Skill
    {
        private Card Owner { get; }
        private IEnumerable<Triggers> Triggers { get; }
        public IEnumerable<int> OpCodes { get; }
        private string Description { get; set; }
        
        public Skill(Card owner, IEnumerable<Triggers> triggers, IEnumerable<int> opCodes, string description)
        {
            Owner = owner;
            Triggers = triggers;
            OpCodes = opCodes;
            Description = description;
        }
    }
}