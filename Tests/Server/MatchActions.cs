using CardGame.Server;

namespace CardGame.Tests.Server
{
    public class MatchActions: BaseTest
    {
        /*
         * Our MATCH RULES Test dealt with what would get a player disqualified but we still need to add the positive
         * tests to check against state as a pincer attack. We only need to cover the happy path in these tests
         * so there is to be less tests overall here.
         */
      
        [Test]
        public void When_A_Player_Draws_A_Card()
        {
            int deckCountBeforeDraw = Player1.Deck.Count;
            int handCountBeforeDraw = Player1.Hand.Count;
            Match.Draw(Player1);
            Assert.IsGreaterThan(deckCountBeforeDraw, Player1.Deck.Count, 
                "Then their deck is reduced in size");
            Assert.IsLessThan(handCountBeforeDraw, Player1.Hand.Count, 
                "Then their hand count is increased in size");
            Assert.IsFalse(Player1.Disqualified);
        }
        
        [Test]
        public void When_A_Unit_Is_Deployed()
        {
            int unitCountBeforeDeploy = Player1.Units.Count;
            int handCountBeforeDeploy = Player1.Hand.Count;
            Card unit = Player1.Hand[0];
            unit.CardType = CardType.Unit;
            Match.Deploy(Player1, unit);
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
            Card support = Player1.Hand[0];
            support.CardType = CardType.Support;
            Match.SetFaceDown(Player1, support);
            Assert.IsEqual(Player1.Supports.Count, supportCountBeforeDeploy + 1, 
                "Their owners support count is increased by 1");
            Assert.IsEqual(Player1.Hand.Count, handCountBeforeDeploy - 1,
                "Their owners hand count is reduced by 1");
        }
        
        [Test]
        public void When_A_Player_Ends_Their_Turn()
        {
            Match.EndTurn(Player1);
            Assert.IsEqual(Player1.State, Player.States.Passive, "Then their state is passive");
        }
    }
}