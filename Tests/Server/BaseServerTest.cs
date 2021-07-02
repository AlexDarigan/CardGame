using System;
using System.Collections.Generic;
using CardGame.Server;
using Godot;
using WAT;

namespace CardGame.Tests.Server
{
    [Start(nameof(Start))]
    [Pre(nameof(Pre))]
    public class BaseServerTest : Test
    {
        /*
         * Basic Custom Test For Our Server Tests. It handles basic initialization. We can be sure it won't ever get
         * run itself as long as its class name and file name do not match. We have no need to modify the deckList
         * because we can just modify the cards on demand for our tests which exist in a controlled environment.
         */

        protected readonly List<SetCodes> _deckList = new();
        private CardRegister Cards;
        protected Match Match;
        protected Player Player1;
        protected Player Player2;

        private void Update()
        {
        }

        private void Queue(int id, CommandId command, params object[] args)
        {
        }

        public void Start()
        {
            GD.Print("Hello World");
            Console.WriteLine("Hello");
            for (int i = 0; i < 40; i++) _deckList.Add(SetCodes.AlphaBioShocker);
        }

        public void Pre()
        {
            Player1 = new Player(1, _deckList);
            Player2 = new Player(2, _deckList);
            Cards = new CardRegister();
            Match = new Match(Player1, Player2, Cards, Update, Queue);
            Match.Begin(new List<Player> {Player1, Player2});
        }

        protected class SkillBuilder
        {
            public string Description = "";
            private readonly List<int> Instructions = new();
            public List<Triggers> Triggers = new();

            public Skill Build(Card owner)
            {
                return new(owner, Triggers, Instructions, Description);
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