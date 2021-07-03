using CardGame.Server;

namespace CardGame.Tests.Server.Scenarios
{
    public class MatchScenario : BaseServerTest
    {
        /*
         * Match Scenario Tests are Tests that contains a sequence of actions that we want to test in order to ensure
         * our game logic works at a interconnected level. We should only ever invoke actions on our match object and
         * we should never mess around with external elements.
         */

        [Test]
        public void Given_A_Game()
        {
            Assert.IsEqual(Player1.State, States.IdleTurnPlayer, "When it starts, Player 1 is Idle");
            Assert.IsEqual(Player2.State, States.Passive, "While Player 2 is Passive");
            int handCountBeforeDraw = Player2.Hand.Count;
            Match.EndTurn(Player1);
            Assert.IsEqual(Player1.State, States.Passive,
                "When Player 1 ends their turn, they are Passive");
            Assert.IsEqual(Player2.State, States.IdleTurnPlayer, "While Player 2 is Idle");
            Assert.IsEqual(Player2.Hand.Count, handCountBeforeDraw + 1,
                "Player 2's Hand Count increased by 1 when Player 1 ended their turn");
            Match.EndTurn(Player1);
            Assert.IsTrue(Player.Disqualified,
                "When player 1 attempts to end their turn during Player 2's turn they are disqualified");
        }
    }
}