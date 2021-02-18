using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using CardGame.Server;

namespace CardGame.Tests.Server
{
    public class BaseTest: WAT.Test
    {
        /*
         * Basic Custom Test For Our Server Tests. It handles basic initialization. We can be sure it won't ever get
         * run itself as long as its class name and file name do not match. We have no need to modify the deckList
         * because we can just modify the cards on demand for our tests which exist in a controlled environment.
         */
        
        private readonly List<SetCodes> _deckList = new List<SetCodes>();
        protected Player Player1;
        protected Player Player2;
        protected Match Match;
        private CardRegister Cards;

        private void Update()
        {
            
        }

        public override void Start()
        {
            for (int i = 0; i < 40; i++)
            {
                _deckList.Add(SetCodes.NullCard);
            }
        }

        public override void Pre()
        {
            Player1 = new Player(1, _deckList);
            Player2 = new Player(2, _deckList);
            Cards = new CardRegister();
            Match = new Match(Player1, Player2, Cards, Update);
        }
        
        protected class SkillBuilder
        {
            public List<Triggers> Triggers = new List<Triggers>();
            private List<int> Instructions = new List<int>();
            public string Description = "";

            public Skill CreateSkill(Card owner)
            {
                return new Skill(owner, Triggers, Instructions, Description);
            }

            public void Add(Instructions instruction)
            {
                Instructions.Add((int) instruction);
            }

            public void Add(int literal)
            {
                Instructions.Add(literal);
            }

            public void Add(Faction faction)
            {
                Instructions.Add((int) faction);
            }
            
        }
    }
}