using System.Net;

namespace CardGame.Tests
{
    public class ConnectionTest: WAT.Test
    {
        private readonly Server.Connection Server = new();
        private readonly Client.Connection Client1 = new();
        private readonly Client.Connection Client2 = new();

        public override string Title()
        {
            return "Given A Live Server";
        }

        public override void Start()
        {
            AddChild(Server);
        }

        [Test]
        public async void And_Two_Clients()
        {
            AddChild(Client1);
            await ToSignal(UntilTimeout(0.2), YIELD);
            Assert.IsEqual(Server.PlayerCount, 1, 
                "When the first client joins there is one player on the server");
            AddChild(Client2);
            await ToSignal(UntilTimeout(0.2), YIELD);
            Assert.IsType<Server.Room>(Server.GetChild(0), 
                "When the second client joins a room is created on the server");
            Assert.IsType<Client.Room>(Client1.GetChild(0),
                "And A Room is created on Client 1");
            Assert.IsType<Client.Room>(Client2.GetChild(0),
                "And A Room is created on Client 2");
            Assert.IsEqual(Client1.GetChild(0).Name, Client2.GetChild(0).Name, 
                "The Client Rooms share the same name");
        }

        public override void End()
        {
            RemoveChild(Client1);
            RemoveChild(Client2);
            RemoveChild(Server);
        }
    }
}