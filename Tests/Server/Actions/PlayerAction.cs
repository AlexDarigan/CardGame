using CardGame.Server;

namespace CardGame.Tests.Server.Actions
{
    public class PlayerAction: BaseTest
    {
        /*
         * We do not care about rules enforcement in this test. We just want to check what happens when
         * we invoke the primary actions on players to see if the correct state exists. We should typically not invoke
         * actions on our match script in this test (unless it can't otherwise be avoided).
         * NOTE: Since these are action based scripts, we don't care about card type
         */

        [Test]
        public void When_A_Player_Draws_A_Card()
        {
            int deckCountBeforeDraw = Player1.Deck.Count;
            int handCountBeforeDraw = Player1.Hand.Count;
            Player1.Draw();
            Assert.IsGreaterThan(deckCountBeforeDraw, Player1.Deck.Count, 
                "Then their deck is reduced in size");
            Assert.IsLessThan(handCountBeforeDraw, Player1.Hand.Count, 
                "Then their hand count is increased in size");
        }
        
        [Test]
        public void When_A_Unit_Is_Deployed()
        {
            int unitCountBeforeDeploy = Player1.Units.Count;
            int handCountBeforeDeploy = Player1.Hand.Count;
            Player1.Deploy(Player1.Hand[0]);
            Assert.IsEqual(Player1.Units.Count, unitCountBeforeDeploy + 1, 
                "Their owners unit count is increased by 1");
            Assert.IsEqual(Player1.Hand.Count, handCountBeforeDeploy - 1,
                "Their owners hand count is reduced by 1");
        }
        
        [Test]
        public void When_A_Card_Is_Set()
        {
            int supportCountBeforeDeploy = Player1.Supports.Count;
            int handCountBeforeDeploy = Player1.Hand.Count;
            Player1.SetFaceDown(Player1.Hand[0]);
            Assert.IsEqual(Player1.Supports.Count, supportCountBeforeDeploy + 1, 
                "Their owners support count is increased by 1");
            Assert.IsEqual(Player1.Hand.Count, handCountBeforeDeploy - 1,
                "Their owners hand count is reduced by 1");
        }
        
        [Test]
        public void When_A_Player_Ends_Their_Turn()
        {
            Match.EndTurn(Player1);
            Assert.IsEqual(Player1.State, States.Passive, "Then their state is passive");
        }
    }
}