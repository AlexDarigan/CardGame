using System.Threading.Tasks;

namespace CardGame.Client.Tests
{
    [Title("Card States")]
    public class CardStates: Fixture
    {
        // Client Card State
        // CanBeActivated
        // CanAttackUnit
        // CanAttackPlayer

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
    }
}