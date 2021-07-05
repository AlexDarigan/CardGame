using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Godot;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace CardGame.Client.Tests
{
    public class Client: WAT.Test
    {
        // Note To Self: Client tests do not work well in-editor due to their reliance on the server
        private static readonly PackedScene MainScene = GD.Load<PackedScene>("res://Global/Main.tscn");
        private Main Game;
        private Room Room1;
        private Room Room2;
        protected Participant P1;
        protected Participant P2;
        
        [Test()]
        public async Task Connection()
        {
            Game = (Main) MainScene.Instance();
            AddChild(Game);
            TestEventData data = await UntilEvent(Game, nameof(Game.GameBegun), 3.0);
            Assert.IsTrue(true, "Reached");
            Players room = (Players) data.Arguments;
            Room1 = room.Room1;
            Room2 = room.Room2;
            P1 = room.Player1;
            P2 = room.Player2;
            Assert.IsTrue(P1 is not null, "Player 1 is not null");
            Assert.IsTrue(P2 is not null, "Player 2 is not null");
            Assert.IsTrue(Room1 is not null, "Room 1 is not null");
            Assert.IsTrue(Room2 is not null, "Room 2 is not null");
        }

        protected async Task Update()
        {
            await UntilEvent(Game, nameof(Game.RoomsUpdated), 3.0);
        }
    }
}