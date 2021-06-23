using CardGame.Server;

namespace CardGame.Tests.Server.Conclusion
{
    /*
     * Game Over Tests test to make sure our victory and lose conditions are working accurately
     */
    
    public class GameOver: BaseTest
    {
        [Test]
        public void A_Player_Tries_To_Draw_From_A_Deck_With_No_Cards_Left()
        {
            // We start with 7 cards already drawn and we don't draw for our first turn
            for (int i = 0; i < 33; i++)
            {
                Match.EndTurn(Player1);
                Match.EndTurn(Player2);
            }
            
            Match.EndTurn(Player1);
            Assert.IsEqual(Player2.Deck.Count, 0);
            Assert.IsEqual(Player2.State, States.Loser);
            Assert.IsEqual(Player1.State, States.Winner);
        }
        
        
    }
}