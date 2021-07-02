using System;
using CardGame.Server;

namespace CardGame.Tests.Server.Actions
{
    public class Battle : BaseServerTest
    {
        [Test]
        public void When_A_Unit_Attacks_Directly()
        {
            Card attacker = Player1.Hand[0];
            attacker.CardType = CardType.Unit;
            attacker.CardState = CardState.Deploy;
            attacker.Power = 1000;
            Match.Deploy(Player1, attacker);
            Match.EndTurn(Player1);
            Match.EndTurn(Player2);
            int healthBeforeDirectAttack = Player2.Health;
            Match.DeclareDirectAttack(Player1, attacker);
            Assert.IsEqual(Player2.Health, healthBeforeDirectAttack - attacker.Power,
                "Then the defending player loses life equal to the Unit's Attack");
        }

        [Test]
        public void When_An_Attacking_Unit_Wins_A_Battle()
        {
            Card attacker = Player1.Hand[0];
            Card defender = Player2.Hand[0];
            attacker.CardType = CardType.Unit;
            attacker.CardState = CardState.Deploy;
            attacker.Power = 2000;
            defender.CardType = CardType.Unit;
            defender.CardState = CardState.Deploy;
            defender.Power = 1000;
            Match.Deploy(Player1, attacker);
            Match.EndTurn(Player1);
            Match.Deploy(Player2, defender);
            Match.EndTurn(Player2);
            Console.WriteLine(attacker.CardState.ToString());
            int healthBeforeAttack = Player2.Health;
            Match.DeclareAttack(Player1, attacker, defender);
            // Assert.IsEqual(Player2.Health, healthBeforeAttack - (attacker.Power - defender.Power), 
            //     "Then the defending player loses life equal to the Attackers Attack - Defenders Attack");
            Assert.Contains(defender, Player2.Graveyard, "And the defending unit is in its owners graveyard");
        }

        [Test]
        public void When_A_Defending_Unit_Wins_A_Battle()
        {
            Card attacker = Player1.Hand[0];
            Card defender = Player2.Hand[0];
            attacker.CardType = CardType.Unit;
            attacker.CardState = CardState.Deploy;
            attacker.Power = 1000;
            defender.CardType = CardType.Unit;
            defender.CardState = CardState.Deploy;
            defender.Power = 2000;
            Match.Deploy(Player1, attacker);
            Match.EndTurn(Player1);
            Match.Deploy(Player2, defender);
            Match.EndTurn(Player2);
            int healthBeforeAttack = Player1.Health;
            Match.DeclareAttack(Player1, attacker, defender);
            Assert.IsEqual(Player1.Health, healthBeforeAttack - (defender.Power - attacker.Power),
                "Then the defending player loses life equal to the Defenders Attack - Attackers Attack");
            Assert.Contains(attacker, Player1.Graveyard, "And the Attacking unit is in its owners graveyard");
        }

        [Test]
        public void When_A_Battle_Ends_In_A_Tie()
        {
            Card attacker = Player1.Hand[0];
            Card defender = Player2.Hand[0];
            attacker.CardType = CardType.Unit;
            attacker.CardState = CardState.Deploy;
            attacker.Power = 1000;
            defender.CardType = CardType.Unit;
            defender.CardState = CardState.Deploy;
            defender.Power = 1000;
            Match.Deploy(Player1, attacker);
            Match.EndTurn(Player1);
            Match.Deploy(Player2, defender);
            Match.EndTurn(Player2);
            int player1HealthBeforeAttack = Player1.Health;
            int player2HealthBeforeAttack = Player2.Health;
            Match.DeclareAttack(Player1, attacker, defender);
            Assert.IsEqual(Player1.Health, player1HealthBeforeAttack,
                "Then the attacking player's health has not changed");
            Assert.IsEqual(Player2.Health, player2HealthBeforeAttack,
                "Then the defending player's health has not changed");
            Assert.Contains(attacker, Player1.Units, "Then the attacker is still on the field");
            Assert.Contains(defender, Player2.Units, "Then the defender is still on the field");
        }
    }
}