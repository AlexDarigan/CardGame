using System.Collections.Generic;
using System.Linq;
using CardGame.Server;

namespace CardGame.Tests.Server
{
    public class OnGameStart: WAT.Test
    {
        private readonly List<SetCodes> DeckList = new List<SetCodes>();
        private Player _player1;
        private Player _player2;
        private Match _match;
        private CardRegister _cards = new CardRegister();
        
        private void Update()
        {
            // We could use this to capture parts of the test   
        }
        public override string Title()
        {
            return "When a game starts";
        }

        public override void Start()
        {
            for (int i = 0; i < 40; i++)
            {
                DeckList.Add(SetCodes.Alpha001);
            }
            _player1 = new Player(1, DeckList);
            _player2 = new Player(2, DeckList);
            _match = new Match(_player1, _player2, _cards, Update);
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

        [Test]
        public void Deck_Contents_As_SetCodes_Equal_DeckList_SetCodes()
        {
            bool success = _player1.Deck.All(card => card.SetCodes == SetCodes.Alpha001) &&
                           _player1.Hand.All(card => card.SetCodes == SetCodes.Alpha001);
            Assert.IsTrue(success);
        }

        
    }
}