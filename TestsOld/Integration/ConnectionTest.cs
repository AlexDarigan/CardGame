using System.Threading.Tasks;
using CardGame.Server;
using WAT;
using ServerConn = CardGame.Server.Connection;
using ClientConn = CardGame.Client.Connection;


namespace CardGame.Tests.Integration
{
    [Title("Given A Live Server")]
    [Start(nameof(Start))]
    [End(nameof(End))]
    public class ConnectionTest : Test
    {
        private readonly ClientConn Client1 = new();
        private readonly ClientConn Client2 = new();
        private readonly ServerConn Server = new();

        public void Start()
        {
            AddChild(Server);
        }

        [Test]
        public async Task And_Two_Clients()
        {
            AddChild(Client1);
            await UntilTimeout(0.5);
            Assert.IsEqual(Server.PlayerCount, 1,
                "When the first client joins there is one player on the server");
            AddChild(Client2);
            await UntilTimeout(0.5);
            Assert.IsType<Room>(Server.GetChild(0),
                "When the second client joins a room is created on the server");
            Assert.IsType<CardGame.Client.Room>(Client1.GetNode("1"),
                "And A Room is created on Client 1");
            Assert.IsType<CardGame.Client.Room>(Client2.GetNode("1"),
                "And A Room is created on Client 2");
            Assert.IsEqual(Client1.GetChild(0).Name, Client2.GetChild(0).Name,
                "The Client Rooms share the same name");
        }

        public void End()
        {
            RemoveChild(Client1);
            RemoveChild(Client2);
            RemoveChild(Server);
        }
    }
}