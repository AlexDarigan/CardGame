using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;

namespace CardGame.Server.Tests
{
    [Title("Operations")]
    public class Operations: Fixture
    {
        [Test(OpCodes.GetDeck, "Deck", 0, 39, "Player drew a card for each card in their deck")]
        [Test(OpCodes.GetHand, "Hand", 0, 14, "Player drew a card for each card in their hand")]
        [Test(OpCodes.GetUnits, "Units", 2, 9, "Player drew a card for each Unit on their field")]
        [Test(OpCodes.GetSupport, "Supports", 0, 8, "Player drew a card for each Support on their field (including the activated card)")]
        [Test(OpCodes.GetGraveyard, "Graveyard", 4, 11, "Player drew a card for each card in their graveyard")]
        public void Getter(OpCodes getZone, string zoneToAddTo, int cardsToAdd, int expected, string context)
        {
            
            StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card support = P1.Hand[0];
            support.Skill = BuildSkill(support, OpCodes.GetController, getZone, OpCodes.Count, OpCodes.GetController, OpCodes.Draw);
            Zone zone = (Zone) typeof(Player).GetProperty(zoneToAddTo)!.GetValue(P1);
            for(int i = 0; i < cardsToAdd; i++) { zone!.Add(new Card(0, P1)); }
            
            Match.SetFaceDown(P1, support);
            Match.EndTurn(P1);
            Match.EndTurn(P2);
            Match.Activate(P1, support);
            
            Assert.IsEqual(P1.Hand.Count, expected, context);
            
        }

        [Test(3, 3, OpCodes.IsEqual, "Opponent drew 5 cards because 3 is equal to 3")]
        [Test(5, 1, OpCodes.IsNotEqual, "Opponent drew 5 cards because 5 is not equal to 1")]
        [Test(9, 2, OpCodes.IsGreaterThan, "Opponent drew 5 cards because 9 is greater than 2")]
        [Test(2, 3, OpCodes.IsLessThan, "Opponent drew 5 cards because 2 is less than 3")]
        [Test(1, 1, OpCodes.And, "Opponent drew 5 cards because 1 AND 1 is true")]
        [Test(1, 0, OpCodes.Or, "Opponent drew 5 cards because 1 OR 0 is true")]
        public void Comparison(int a, int b, OpCodes comparison, string context)
        {
            StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card support = P1.Hand[0];
            const int jump = 4;
            support.Skill = BuildSkill(support, OpCodes.Literal, a, OpCodes.Literal, b, comparison, OpCodes.Literal, jump, OpCodes.If, OpCodes.Literal, 5, OpCodes.GetOpponent, OpCodes.Draw);
            Match.SetFaceDown(P1, support);
            Match.EndTurn(P1);
            Match.EndTurn(P2);
            
            int count = P2.Hand.Count;
            Match.Activate(P1, support);
            
            Assert.IsEqual(P2.Hand.Count, count + 5, context);
        }

        [Test(OpCodes.Add, 500, 500, 1000, "Controllers health is set to the result of 500 + 500")]
        [Test(OpCodes.Subtract, 1000, 500, 500, "Controllers health is set to the result of 1000 - 500")]
        [Test(OpCodes.Multiply, 2, 1000, 2000, "Controllers health is set to the result of 2 x 1000")]
        [Test(OpCodes.Divide, 1000, 2, 500, "Controllers health is set to the result of 1000 / 2")]
        public void Calculation(OpCodes math, int a, int b, int result, string context)
        {
            StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card support = P1.Hand[0];
            support.Skill = BuildSkill(support, OpCodes.Literal, a, OpCodes.Literal, b, math, OpCodes.GetController, OpCodes.SetHealth);
            int health = P1.Health;
            
            Match.SetFaceDown(P1, support);
            Match.EndTurn(P1);
            Match.EndTurn(P2);
            Match.Activate(P1, support);
            
            Assert.IsEqual(P1.Health, result, context);
        }
        
        
        [Test]
        public void ActivationAction()
        {
            // How do we remove the current
            StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card support = P1.Hand[0];
            Match.SetFaceDown(P1, support);
            Match.EndTurn(P1);
            Match.EndTurn(P2);
            int handCount = P1.Hand.Count;
            int deckCount = P1.Deck.Count;
            
            Match.Activate(P1, support);
            
            Assert.IsEqual(P1.Hand.Count, handCount + 2, "Player added 2 cards to their hand");
            Assert.IsEqual(P1.Deck.Count, deckCount - 2, "Player removed the drawn cards from their deck");
            Assert.Contains(support, P1.Graveyard, "Support was moved to graveyard");
        }
        
        // ACTIVATED CARD
        // -> PASS PLAY
        // -> RESOLVE
        // -> SPAWN EFFECT
        // -> STATE CHANGES
    
      
    }
}