namespace CardGame.Server.Tests
{
    [Title("Player Actions")]
    public class PlayerActions: Fixture
    {
        // Focus on Player Actions
        // Not Rules
        
        [Test()]
        public void Draw()
        {
            StartGame(BuildDeck(SetCodes.AlphaBioShocker));
            int deckCount = P1.Deck.Count;
            int handCount = P1.Hand.Count;
            P1.Draw();
            Assert.IsEqual(P1.Hand.Count, handCount + 1, "The Player's hand count increased by 1 Card");
            Assert.IsEqual(P1.Deck.Count, deckCount - 1, "The player's deck count decreased by 1 Card");
        }

        [Test]
        public void DeployAction()
        {
            StartGame(BuildDeck(SetCodes.AlphaBioShocker));
            int handCount = P1.Hand.Count;
            int unitsCount = P1.Units.Count;
            P1.Deploy(P1.Hand[0]);
            Assert.IsEqual(P1.Units.Count, unitsCount + 1, "The player's units count increased by 1 Card");
            Assert.IsEqual(P1.Hand.Count, handCount - 1, "The player's hand count decreased by 1 card");
        }
        
        [Test]
        public void SetFaceDownAction()
        {
            StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            int handCount = P1.Hand.Count;
            int supportsCount = P1.Supports.Count;
            P1.SetFaceDown(P1.Hand[0]);
            Assert.IsEqual(P1.Supports.Count, supportsCount + 1, "The player's supports count increased by 1 Card");
            Assert.IsEqual(P1.Hand.Count, handCount - 1, "The player's hand count decreased by 1 card");
        }
    }
}