namespace CardGame.Tests
{
    public class ConnectionTest: WAT.Test
    {
        private Server.Connection Server = new();
        private Client.Connection Client1 = new();
        private Client.Connection Client2 = new();

        public override string Title()
        {
            return "Given A Live Server";
        }

        public override void Start()
        {
            AddChild(Server);
        }

        [Test]
        public async void When_The_First_Client_Joins()
        {
            AddChild(Client1);
            await ToSignal(UntilTimeout(0.2), YIELD);
            Assert.IsEqual(Server.PlayerCount, 1, 
                "Then there is one player on the server");
            
        }
    }
}