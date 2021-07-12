namespace CardGame.Server.Tests
{
    [Title("Link")]
    public class Link: Fixture
    {
        //     
        // ACTIVATED CARD
        // -> PASS PLAY
        // -> RESOLVE
        // -> SPAWN EFFECT
        // -> STATE CHANGES
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
            //Assert.IsEqual(P1.Deck.Count, deckCount - 2, "Player removed the drawn cards from their deck");
            //Assert.Contains(support, P1.Graveyard, "Support was moved to graveyard");
        }
    }
}