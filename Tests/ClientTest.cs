using System.Threading.Tasks;

namespace CardGame.Tests
{
    public class ClientTest: WAT.Test
    {
        public override string Title()
        {
            return "Client";
        }

        private readonly Server.Connection Server = new Server.Connection();
        private readonly Client.Connection Client = new Client.Connection();

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