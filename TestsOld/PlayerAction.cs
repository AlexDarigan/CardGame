namespace CardGame.Tests.Server.Actions
{
    public class PlayerAction : BaseServerTest
    {
        [Test]
        public void When_A_Player_Ends_Their_Turn()
        {
            Match.EndTurn(Player1);
            Assert.IsEqual(Player1.State, States.Passive, "Then their state is passive");
        }
    }
}