namespace CardGame.Server.Tests
{
    [Title("Game Conclusions")]
    public class Conclusion: Fixture
    {
        [Test()]
        public void DeckedOut()
        {
            Describe("Player 2 lost due to Deck Out");
            // Keep in mind due to Client constraints, we have a hand limit of 9 max
            StartGame();
            for (int i = 0; i < 34; i++)
            {
                Match.EndTurn(P1);
                Match.EndTurn(P2);
            }
            
            Assert.IsTrue(Match.GameOver, "Game is over");
            Assert.IsEqual(P1.State, States.Winner, "Player 1 won");
            Assert.IsEqual(P2.State, States.Loser, "Player 2 lost");
            Assert.IsEqual(P2.Deck.Count, 0, "Player 2 has no cards left in their deck");
        }

        [Test()]
        public void HealthRanOut()
        {
            Describe("Player 2 lost due to health reaching 0");
            StartGame(BuildDeck(SetCodes.BasicUnit));
            Card unit = P1.Hand[0];
            Match.Deploy(P1, unit);
            Match.EndTurn(P1);
            Match.EndTurn(P2);
            int health = P2.Health;
            unit.Power = health;
            Match.DeclareDirectAttack(P1, unit);
            
            Assert.IsTrue(Match.GameOver, "Game is over");
            Assert.IsEqual(P1.State, States.Winner, "Player 1 won");
            Assert.IsEqual(P2.State, States.Loser, "Player 2 lost");
            Assert.IsEqual(P2.Health, 0, "Player 2's health reached 0");
        }
    }
}