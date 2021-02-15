using ServerConn = CardGame.Server.Connection;
using ClientConn = CardGame.Client.Connection;

namespace CardGame.Tests.Client
{
    public class ClientConnection: WAT.Test
    {
        public override string Title()
        {
            return "Client";
        }

        private readonly ServerConn Server = new ServerConn();
        private readonly ClientConn Client = new ClientConn();

        public override void Start()
        {
            AddChild(Server);
            AddChild(Client);
        }

        [Test]
        public async void Is_Live()
        {
            await ToSignal(UntilTimeout(0.5), YIELD);
            Assert.IsTrue(Client.IsLive);
        }

        [Test]
        public void Is_Client()
        {
            Assert.IsTrue(Client.IsClient);
        }

        public override void End()
        {
            RemoveChild(Client);
            RemoveChild(Server);
        }
    }
}