using System.Security.Cryptography;

namespace CardGame.Server.Tests
{
    // This is likely to be one of our more messy tests so I'll guess we should just bear with it
    [Title("Battle Calculation")]
    public class Battle : Fixture
    {
        [Test()]
        public void WinBattle()
        {
            Describe("When an attacking Unit wins a Battle");
            StartGame(BuildDeck(SetCodes.AlphaBioShocker), BuildDeck(SetCodes.AlphaBioShocker));
            Card attacker = P1.Hand[0];
            Card defender = P2.Hand[1];
            Match.Deploy(P1, attacker);
            Match.EndTurn(P1);
            Match.Deploy(P2, defender);
            Match.EndTurn(P2);
            
            attacker.Power += 500;
            int lifeCount = P2.Health;
            Match.DeclareAttack(P1, attacker, defender);
            int difference = attacker.Power - defender.Power;
            Assert.IsFalse(attacker.IsReady, "Attacker is exhausted");
            Assert.IsGreaterThan(attacker.Power, defender.Power, "The attackers power is greater than the defender's power");
            Assert.Contains(defender, P2.Graveyard, "The defending Unit was sent to the graveyard");
            Assert.IsEqual(lifeCount - difference, P2.Health, "The difference in power was subtracted from the users");
            Assert.IsEqual(attacker.CardState, CardState.None, $"Attacker is in {CardState.None}");
        }

        [Test()]
        public void LoseBattle()
        {
            Describe("When an attack Unit loses a battle");
            StartGame(BuildDeck(SetCodes.AlphaBioShocker), BuildDeck(SetCodes.AlphaBioShocker));
            Card attacker = P1.Hand[0];
            Card defender = P2.Hand[1];
            Match.Deploy(P1, attacker);
            Match.EndTurn(P1);
            Match.Deploy(P2, defender);
            Match.EndTurn(P2);
            
            defender.Power += 500;
            int lifeCount = P1.Health;
            Match.DeclareAttack(P1, attacker, defender);
            int difference = defender.Power - attacker.Power;
            Assert.IsGreaterThan(defender.Power, attacker.Power, "The defenders power is greater than attackers power's power");
            Assert.Contains(attacker, P1.Graveyard, "The attacking Unit was sent to the graveyard");
            Assert.IsEqual(lifeCount - difference, P1.Health, "The difference in power was subtracted from the attacking player users");
            Assert.IsEqual(attacker.CardState, CardState.None, $"Attacker is in {CardState.None}");
        }
        
        [Test()]
        public void TieBattle()
        {
            Describe("When a battle ends in a tie");
            StartGame(BuildDeck(SetCodes.AlphaBioShocker), BuildDeck(SetCodes.AlphaBioShocker));
            Card attacker = P1.Hand[0];
            Card defender = P2.Hand[1];
            Match.Deploy(P1, attacker);
            Match.EndTurn(P1);
            Match.Deploy(P2, defender);
            Match.EndTurn(P2);

            int p1Health = P1.Health;
            int p2Health = P2.Health;
            Match.DeclareAttack(P1, attacker, defender);
            
            Assert.IsEqual(P1.Health, p1Health, "Player 1 did not lose health");
            Assert.IsEqual(P1.Health, p2Health, "Player 2 did not lose health");
            Assert.IsFalse(attacker.IsReady, "Attacker is exhausted");
            Assert.Contains(attacker, P1.Units, "Attacker is still on the field");
            Assert.Contains(defender, P2.Units, "Defender is still on the field");
            Assert.IsEqual(attacker.CardState, CardState.None, $"Attacker is in {CardState.None}");
        }

        [Test()]
        public void DirectAttack()
        {
            Describe("When a unit attacks directly");
            StartGame(BuildDeck(SetCodes.AlphaBioShocker));
            Card attacker = P1.Hand[0];
            Match.Deploy(P1, attacker);
            Match.EndTurn(P1);
            Match.EndTurn(P2);
            int health = P2.Health;
            
            Match.DeclareDirectAttack(P1, attacker);
            
            Assert.IsEqual(P2.Health, health - attacker.Power, "Defending player lost health equal to attacker");
            Assert.IsFalse(attacker.IsReady, "Attacker is exhausted");
            Assert.IsEqual(attacker.CardState, CardState.None, $"Attacker is in {CardState.None}");
        }
    }
}

