using System.Collections.Generic;
using CardGame.Server;

namespace CardGame.Tests.Server
{
    public class Battle: WAT.Test
    {
        private readonly List<SetCodes> _deckList = new List<SetCodes>();
        private Player _player1;
        private Player _player2;
        private Match _match;
        private CardRegister _cards;

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
            _player1 = new Player(1, _deckList);
            _player2 = new Player(2, _deckList);
            _cards = new CardRegister();
            _match = new Match(_player1, _player2, _cards, Update);
        }

        [Test]
        public void When_A_Unit_Attacks_Directly()
        {
            Card attacker = _player1.Hand[0];
            attacker.CardType = CardType.Unit;
            attacker.Power = 1000;
            _match.Deploy(_player1, attacker);
            _match.EndTurn(_player1);
            _match.EndTurn(_player2);
            int healthBeforeDirectAttack = _player2.Health;
            _match.DeclareDirectAttack(_player1, attacker);
            Assert.IsEqual(_player2.Health, healthBeforeDirectAttack - attacker.Power, 
                "Then the defending player loses life equal to the Unit's Attack");
        }

    }
}