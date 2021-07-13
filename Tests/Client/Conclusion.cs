using System;
using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Tests
{
    [Title("Conclusion")]
    public class Conclusion: Fixture
    {
        [Test()]
        public async Task WhenPlayer2DecksOut()
        {

            await StartGame();
            Label gameOver1 = (Label) Room1.Text.Get("GameOver");
            Label gameOver2 = (Label) Room2.Text.Get("GameOver");

            int drawUntilDeckOut = P2.Deck.Count + 1;

            for (int i = 0; i < drawUntilDeckOut; i++) { await Queue(P1Input.OnPassPlayPressed, P2Input.OnPassPlayPressed); }
            
            Assert.IsTrue(gameOver1.Visible, "Then Game Over Label is Visible in Room 1");
            Assert.IsTrue(gameOver2.Visible, "Then Game Over Label is Visible in Room 2");
            Assert.IsEqual(gameOver1.Text, "You Win!", "And Player 1 won");
            Assert.IsEqual(gameOver2.Text, "You Lose!", "And Player 2 lost");
            Assert.IsEqual(P2.Deck.Count, 0, "And Player 2's Deck has 0 cards left");
        }
    }
}