using System;
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
            Assert.IsEqual(card.CardType, CardTypes.Unit, "When it is a Unit Card");
            Assert.IsEqual(P1Input.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.IsEqual(card.CurrentZone, card.Controller.Hand, "And it is in their controller's hand");
            Assert.IsLessThan(card.Controller.Units.Count, 5, "And its Controller's Unit Zones is not full");
            Assert.IsEqual(card.CardState, CardGame.CardStates.Deploy, "Then they can be deployed");
        }
        
        [Test]
        public async Task SetFaceDown()
        {
            await StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card card = P1.Hand[0];
            Assert.IsEqual(card.CardType, CardTypes.Support, "When it is a Support Card");
            Assert.IsEqual(P1Input.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.IsEqual(card.CurrentZone, card.Controller.Hand, "And it is in its controller's hand");
            Assert.IsLessThan(card.Controller.Supports.Count, 5, "And its Controller's Support Zones is not full");
            Assert.IsEqual(card.CardState, CardGame.CardStates.SetFaceDown, "Then it can be set face down");
        }
        
        [Test]
        public async Task Activation()
        {
            // We can't really see this happen but maybe that's just the nature of it
            await StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card card = P1.Hand[0];
            
            await Queue(() => P1Input.OnCardPressed(card), P1Input.EndTurn, P2Input.EndTurn);
            
            Assert.IsEqual(card.CardType, CardTypes.Support, "When it is a Support Card");
            Assert.IsEqual(P1Input.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.IsEqual(card.CurrentZone, card.Controller.Supports, "And it is in its controller's support");
            Assert.IsEqual(card.CardState, CardGame.CardStates.Activate, "Then it can be activated");
        }
        
        [Test]
        public async Task AttackUnit()
        {
            await StartGame(BuildDeck(SetCodes.AlphaBioShocker), BuildDeck(SetCodes.AlphaBioShocker));
            Card attacker = P1.Hand[0];
            Card defender = P2.Hand[0];
            
            await Queue(() => P1Input.OnCardPressed(attacker), P1Input.EndTurn, () => P2Input.OnCardPressed(defender), P2Input.EndTurn);

            Assert.IsEqual(attacker.CardType, CardTypes.Unit, "When it is a Unit Card");
            Assert.IsEqual(P1Input.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.IsEqual(attacker.CurrentZone, P1.Units, "And it is in its controller's units");
            Assert.IsGreaterThan(P2.Units.Count, 0, "And its controller's opponent's Unit zone is not empty");
            Assert.IsEqual(attacker.CardState, CardGame.CardStates.AttackUnit, "Then it can attack target unit");
        }
        
        [Test]
        public async Task AttackPlayer()
        {
            await StartGame(BuildDeck(SetCodes.AlphaBioShocker));
            Card card = P1.Hand[0];

            await Queue(() => P1Input.OnCardPressed(card), P1Input.EndTurn, P2Input.EndTurn);
            
            Assert.IsEqual(card.CardType, CardTypes.Unit, "When it is a Unit Card");
            Assert.IsEqual(P1Input.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.IsEqual(card.CurrentZone, P1.Units, "And it is in its controller's units");
            Assert.IsEqual(P2.Units.Count, 0, "And its controller's opponent's Unit zone is empty");
            Assert.IsEqual(card.CardState, CardGame.CardStates.AttackPlayer, "Then it can attack directly");
        }
        
    }
}