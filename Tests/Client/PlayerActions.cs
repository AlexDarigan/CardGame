using System.Threading.Tasks;

namespace CardGame.Client.Tests
{
    [Title("Player Actions")]
    public class PlayerActions: Fixture
    {
        [Test]
        public async Task Deploy()
        {
            await StartGame(BuildDeck(SetCodes.AlphaBioShocker));
            int handCount = P1.Hand.Count;
            int unitsCount = P2.Units.Count;
            
            await Queue(() => P1Input.OnCardPressed(P1.Hand[0]));
            
            Assert.IsEqual(P1.Units.Count, unitsCount + 1, "The player's units count increased by 1 Card");
            Assert.IsEqual(P1.Hand.Count, handCount - 1, "The player's hand count decreased by 1 card");
        }

        [Test]
        public async Task SetFaceDown()
        {
            await StartGame(BuildDeck(SetCodes.AlphaQuestReward));
            int handCount = P1.Hand.Count;
            int supportsCount = P1.Supports.Count;
            
            await Queue(() => P1Input.OnCardPressed(P1.Hand[0]));
            
            Assert.IsEqual(P1.Supports.Count, supportsCount + 1, "The player's supports count increased by 1 Card");
            Assert.IsEqual(P1.Hand.Count, handCount - 1, "The player's hand count decreased by 1 card");
        }
        
    }
}