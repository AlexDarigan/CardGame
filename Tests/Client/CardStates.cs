﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CardGame.Client.Tests
{
    [Title("Card States")]
    public class CardStates: Fixture
    {
        [Test]
        public async Task Deploy()
        {
            await StartGame(BuildDeck(SetCodes.AlphaBioShocker));
            Card card = P1.Hand[0];
            Assert.IsEqual(card.CardType, CardType.Unit, "When it is a Unit Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, card.Controller.Hand, "And it is in their controller's hand");
            Assert.IsLessThan(card.Controller.Units.Count, 5, "And its Controller's Unit Zones is not full");
            Assert.IsEqual(card.CardState, CardState.Deploy, "Then they can be deployed");
        }
        
        [Test]
        public async Task SetFaceDown()
        {
            await StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card card = P1.Hand[0];
            Assert.IsEqual(card.CardType, CardType.Support, "When it is a Support Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, card.Controller.Hand, "And it is in its controller's hand");
            Assert.IsLessThan(card.Controller.Supports.Count, 5, "And its Controller's Support Zones is not full");
            Assert.IsEqual(card.CardState, CardState.Set, "Then it can be set face down");
        }
        
        [Test]
        public async Task Activation()
        {
            // We can't really see this happen but maybe that's just the nature of it
            await StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card card = P1.Hand[0];
            
            await Queue(() => P1.OnCardPressed(card), P1.EndTurn, P2.EndTurn);
            
            Assert.IsEqual(card.CardType, CardType.Support, "When it is a Support Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, card.Controller.Supports, "And it is in its controller's support");
            Assert.IsEqual(card.CardState, CardState.Activate, "Then it can be activated");
        }
        
        [Test]
        public async Task AttackUnit()
        {
            await StartGame(BuildDeck(SetCodes.AlphaBioShocker), BuildDeck(SetCodes.AlphaBioShocker));
            Card attacker = P1.Hand[0];
            Card defender = P2.Hand[0];
            
            await Queue(() => P1.OnCardPressed(attacker), P1.EndTurn, () => P2.OnCardPressed(defender), P2.EndTurn);
//            await Update();

            Assert.IsEqual(attacker.CardType, CardType.Unit, "When it is a Unit Card");
            Assert.IsEqual(attacker.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(attacker, P1.Units, "And it is in its controller's units");
            Assert.IsGreaterThan(P2.Units.Count, 0, "And its controller's opponent's Unit zone is not empty");
            Assert.IsEqual(attacker.CardState, CardState.AttackUnit, "Then it can attack target unit");
        }
        
        [Test]
        public async Task AttackPlayer()
        {
            await StartGame(BuildDeck(SetCodes.AlphaBioShocker));
            Card card = P1.Hand[0];

            await Queue(() => P1.OnCardPressed(card), P1.EndTurn, P2.EndTurn);
            
            Assert.IsEqual(card.CardType, CardType.Unit, "When it is a Unit Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, P1.Units, "And it is in its controller's units");
            Assert.IsEqual(P2.Units.Count, 0, "And its controller's opponent's Unit zone is empty");
            Assert.IsEqual(card.CardState, CardState.AttackPlayer, "Then it can attack directly");
        }
        
    }
}