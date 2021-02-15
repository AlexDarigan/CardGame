using System.Linq;

namespace CardGame.Tests.Server.Initialization
{
    public class OnGameStart: BaseTest
    {
      
        public override string Title()
        {
            return "When a game starts";
        }
        
        [Test]
        public void Player_Decks_Contain_33_Cards()
        {
            Assert.IsEqual(Player1.Deck.Count, 33);
            Assert.IsEqual(Player2.Deck.Count, 33);
        }

        [Test]
        public void Player_Hands_Contain_7_Cards()
        {
            // We may need to update this to account for first-turn draw..
            // ..unless we choose to not allow the first turn draw
            Assert.IsEqual(Player1.Hand.Count, 7);
            Assert.IsEqual(Player2.Hand.Count, 7);
        }

        [Test]
        public void Deck_Contents_As_SetCodes_Equal_DeckList_SetCodes()
        {
            bool success = Player1.Deck.All(card => card.SetCodes == SetCodes.NullCard) &&
                           Player1.Hand.All(card => card.SetCodes == SetCodes.NullCard);
            Assert.IsTrue(success);
        }

        
    }
}