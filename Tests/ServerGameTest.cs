using CardGame.Server;

namespace CardGame.Tests
{
    public class ServerGameTest: WAT.Test
    {
        private readonly Player _player1 = new Player(1);
        private readonly Player _player2 = new Player(2);
        private Room _room;
        
        public override string Title()
        {
            return "When a game starts";
        }

        public override void Start()
        {
            _room = new Room(_player1, _player2);
        }

        [Test]
        public void Player_Decks_Contain_33_Cards()
        {
            Assert.IsEqual(_player1.Deck.Count, 33);
            Assert.IsEqual(_player2.Deck.Count, 33);
        }

        [Test]
        public void Player_Hands_Contain_7_Cards()
        {
            // We may need to update this to account for first-turn draw..
            // ..unless we choose to not allow the first turn draw
            Assert.IsEqual(_player1.Hand.Count, 7);
            Assert.IsEqual(_player2.Hand.Count, 7);
        }

        public override void End()
        {
            _room.Free();
        }
    }
}