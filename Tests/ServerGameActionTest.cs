using System.Collections.Generic;
using CardGame.Server;

namespace CardGame.Tests
{
    public class ServerGameActionTest: WAT.Test
    {
       
        private void Update()
        {
            
        }
        
        [Test]
        public void Given_A_Game()
        {
            List<SetCodes> deckList = new List<SetCodes>();
            for (int i = 0; i < 40; i++)
            {
                deckList.Add(SetCodes.Alpha001);
            }
            Player player1 = new Player(1, deckList);
            Player player2 = new Player(2, deckList);
            CardRegister cards = new CardRegister();
            Match match = new Match(player1, player2, cards, Update);
          
            
            Assert.IsEqual(player1.State, Player.States.Idle, "When it starts, Player 1 is Idle");
            Assert.IsEqual(player2.State, Player.States.Passive, "While Player 2 is Passive");
            int handCountBeforeDraw = player2.Hand.Count;
            match.EndTurn(player1);
            Assert.IsEqual(player1.State, Player.States.Passive, 
                "When Player 1 ends their turn, they are Passive");
            Assert.IsEqual(player2.State, Player.States.Idle, "While Player 2 is Idle");
            Assert.IsEqual(player2.Hand.Count, handCountBeforeDraw + 1, 
                "Player 2's Hand Count increased by 1 when Player 1 ended their turn");
            match.EndTurn(player1);
            Assert.IsTrue(player1.Disqualified, 
                "When player 1 attempts to end their turn during Player 2's turn they are disqualified");
        }
    }
}