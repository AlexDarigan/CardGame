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
            int drawUntilDeckOut = P2.Deck.Count + 1;

            for (int i = 0; i < drawUntilDeckOut; i++) { await Queue(P1.EndTurn, P2.EndTurn); }
            
            Assert.IsTrue(Room1.GUI.GameOver.Visible, "Then Game Over Label is Visible in Room 1");
            Assert.IsTrue(Room2.GUI.GameOver.Visible, "Then Game Over Label is Visible in Room 2");
            Assert.IsEqual(Room1.GUI.GameOver.Text, "You Win!", "And Player 1 won");
            Assert.IsEqual(Room2.GUI.GameOver.Text, "You Lose!", "And Player 2 lost");
            Assert.IsEqual(P2.Deck.Count, 0, "And Player 2's Deck has 0 cards left");

        }
    }
}