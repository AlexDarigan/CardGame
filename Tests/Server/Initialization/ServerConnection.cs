using CardGame.Server;
using WAT;

namespace CardGame.Tests.Server.Initialization
{
    [Title("Server")]
    [Start(nameof(Start))]
    [End(nameof(End))]
    public class ServerConnection : Test
    {
        private readonly Connection Server = new();

        public void Start()
        {
            AddChild(Server);
        }

        [Test]
        public void Is_Live()
        {
            Assert.IsTrue(Server.IsLive);
        }

        [Test]
        public void Is_Network_Server()
        {
            Assert.IsTrue(Server.IsServer);
        }

        public void End()
        {
            RemoveChild(Server);
            Server.Free();
        }
    }
}