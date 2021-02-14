using CardGame.Server;
using Godot;

namespace CardGame.Tests
{
    public class ServerTest: WAT.Test
    {
        private readonly Connection Server = new Connection();

        public override string Title()
        {
            return "Server";
        }

        public override void Start()
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

        public override void End()
        {
            RemoveChild(Server);
            Server.Free();
        }
    }
}