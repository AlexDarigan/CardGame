using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public void GetterOperations(OpCodes getZone, string zoneToAddTo, int cardsToAdd, int expected, string context)
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