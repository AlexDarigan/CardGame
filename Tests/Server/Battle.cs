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
        
        // Attacker > Defender; Defender.Owner Loses Life; Defender Is Destroyed;
        // Defender < Attacker; Attacker.Controller Loses Life, Attacker Is Destroyed;

        [Test]
        public void When_An_Attacking_Unit_Wins_A_Battle()
        {
            Card attacker = _player1.Hand[0];
            Card defender = _player2.Hand[0];
            attacker.CardType = CardType.Unit;
            attacker.Power = 2000;
            defender.CardType = CardType.Unit;
            defender.Power = 1000;
            _match.Deploy(_player1, attacker);
            _match.EndTurn(_player1);
            _match.Deploy(_player2, defender);
            _match.EndTurn(_player2);
            int healthBeforeAttack = _player2.Health;
            _match.DeclareAttack(_player1, attacker, defender);
            Assert.IsEqual(_player2.Health, healthBeforeAttack - (attacker.Power - defender.Power), 
                "Then the defending player loses life equal to the Attackers Attack - Defenders Attack");
            Assert.Contains(defender, _player2.Graveyard, "And the defending unit is in its owners graveyard");
        }
        
        [Test]
        public void When_A_Defending_Unit_Wins_A_Battle()
        {
            Card attacker = _player1.Hand[0];
            Card defender = _player2.Hand[0];
            attacker.CardType = CardType.Unit;
            attacker.Power = 1000;
            defender.CardType = CardType.Unit;
            defender.Power = 2000;
            _match.Deploy(_player1, attacker);
            _match.EndTurn(_player1);
            _match.Deploy(_player2, defender);
            _match.EndTurn(_player2);
            int healthBeforeAttack = _player1.Health;
            _match.DeclareAttack(_player1, attacker, defender);
            Assert.IsEqual(_player1.Health, healthBeforeAttack - (defender.Power - attacker.Power), 
                "Then the defending player loses life equal to the Defenders Attack - Attackers Attack");
            Assert.Contains(attacker, _player1.Graveyard, "And the Attacking unit is in its owners graveyard");
        }
        
        [Test]
        public void When_A_Battle_Ends_In_A_Tie()
        {
            Card attacker = _player1.Hand[0];
            Card defender = _player2.Hand[0];
            attacker.CardType = CardType.Unit;
            attacker.Power = 1000;
            defender.CardType = CardType.Unit;
            defender.Power = 1000;
            _match.Deploy(_player1, attacker);
            _match.EndTurn(_player1);
            _match.Deploy(_player2, defender);
            _match.EndTurn(_player2);
            int player1HealthBeforeAttack = _player1.Health;
            int player2HealthBeforeAttack = _player2.Health;
            _match.DeclareAttack(_player1, attacker, defender);
            Assert.IsEqual(_player1.Health, player1HealthBeforeAttack, 
                "Then the attacking player's health has not changed");
            Assert.IsEqual(_player2.Health, player2HealthBeforeAttack,
                "Then the defending player's health has not changed");
            Assert.Contains(attacker, _player1.Units, "Then the attacker is still on the field");
            Assert.Contains(defender, _player2.Units, "Then the defender is still on the field");
        }

    }
}