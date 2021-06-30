﻿using System.Threading.Tasks;
using ServerConn = CardGame.Server.Connection;
using ClientConn = CardGame.Client.Connection;


namespace CardGame.Tests.Client
{
    [Title("Client")]
    [Start(nameof(Start))]
    [End(nameof(End))]
    public class ClientConnection: WAT.Test
    {
        private readonly ServerConn Server = new ServerConn();
        private readonly ClientConn Client = new ClientConn();

        public void Start()
        {
            AddChild(Server);
            AddChild(Client);
        }

        [Test]
        public async Task Is_Live()
        {
            await UntilTimeout(0.5);
            Assert.IsTrue(Client.IsLive);
        }

        [Test]
        public void Is_Client()
        {
            Assert.IsTrue(Client.IsClient);
        }

        public void End()
        {
            RemoveChild(Client);
            RemoveChild(Server);
        }
    }
}