namespace CardGame.Server.Tests
{
    [Title("Card States")]
    public class CardStates: ServerFixture
    {
        [Test]
        public void Deploy()
        {
            StartGame(BuildDeck(SetCodes.AlphaBioShocker));
            Card card = P1.Hand[0];
            Assert.IsEqual(card.CardType, CardType.Unit, "When it is a Unit Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, card.Controller.Hand, "And it is in their controller's hand");
            Assert.IsLessThan(card.Controller.Units.Count, 5, "And its Controller's Unit Zones is not full");
            Assert.IsEqual(card.CardState, CardState.Deploy, "Then they can be deployed");
        }

        [Test]
        public void SetFaceDown()
        {
            StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card card = P1.Hand[0];
            Assert.IsEqual(card.CardType, CardType.Support, "When it is a Support Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, card.Controller.Hand, "And it is in its controller's hand");
            Assert.IsLessThan(card.Controller.Supports.Count, 5, "And its Controller's Support Zones is not full");
            Assert.IsEqual(card.CardState, CardState.Set, "Then it can be set face down");
        }
        
        [Test]
        public void Activation()
        {
            StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            Card card = P1.Hand[0];
            Match.SetFaceDown(P1, card);
            Match.EndTurn(P1);
            Match.EndTurn(P2);
            Assert.IsEqual(card.CardType, CardType.Support, "When it is a Support Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, card.Controller.Supports, "And it is in its controller's support");
            Assert.IsTrue(card.IsReady, "And it is ready");
            Assert.IsEqual(card.CardState, CardState.Activate, "Then it can be activated");
        }

        [Test]
        public void AttackUnit()
        {
            StartGame(BuildDeck(SetCodes.AlphaBioShocker), BuildDeck(SetCodes.AlphaBioShocker));
            Card card = P1.Hand[0];
            Match.Deploy(P1, card);
            Match.EndTurn(P1);
            Card defender = P2.Hand[1];
            Match.Deploy(P2, defender);
            Match.EndTurn(P2);
            Assert.IsEqual(card.CardType, CardType.Unit, "When it is a Unit Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, P1.Units, "And it is in its controller's units");
            Assert.IsTrue(card.IsReady, "And it is ready");
            Assert.IsGreaterThan(P2.Units.Count, 0, "And its controller's opponent's Unit zone is not empty");
            Assert.IsEqual(card.CardState, CardState.AttackUnit, "Then it can attack target unit");
        }

        [Test]
        public void AttackPlayer()
        {
            StartGame(BuildDeck(SetCodes.AlphaBioShocker));
            Card card = P1.Hand[0];
            Match.Deploy(P1, card);
            Match.EndTurn(P1);
            Match.EndTurn(P2);
            Assert.IsEqual(card.CardType, CardType.Unit, "When it is a Unit Card");
            Assert.IsEqual(card.Controller.State, States.IdleTurnPlayer, "And its controller is the Idle Turn Player");
            Assert.Contains(card, P1.Units, "And it is in its controller's units");
            Assert.IsTrue(card.IsReady, "And it is ready");
            Assert.IsEqual(P2.Units.Count, 0, "And its controller's opponent's Unit zone is empty");
            Assert.IsEqual(card.CardState, CardState.AttackPlayer, "Then it can attack directly");
        }
    }
}