using System.Collections.Generic;
using CardGame.Server;

namespace CardGame.Tests.Server
{
    /*
     * Game Over Tests test to make sure our victory and lose conditions are working accurately
     */
    
    public class GameOver: WAT.Test
    {
        private void Update()
        {
            
        }

        private readonly List<SetCodes> _deckList = new List<SetCodes>();
        private Player _player1;
        private Player _player2;
        private Match _match;
        private CardRegister _cards;

        public override string Title()
        {
            return "The Game Is Over When";
        }

        public override void Start()
        {
            for (int i = 0; i < 40; i++)
            {
                _deckList.Add(SetCodes.NullCard);
            }
        }

        public override void Pre()
        {
            _player1 = new Player(1, _deckList);
            _player2 = new Player(2, _deckList);
            _cards = new CardRegister();
            _match = new Match(_player1, _player2, _cards, Update);
        }

        [Test]
        public void A_Player_Tries_To_Draw_From_A_Deck_With_No_Cards_Left()
        {
            // We start with 7 cards already drawn and we don't draw for our first turn
            for (int i = 0; i < 33; i++)
            {
                _match.EndTurn(_player1);
                _match.EndTurn(_player2);
            }
            
            _match.EndTurn(_player1);
            Assert.IsEqual(_player2.Deck.Count, 0);
            Assert.IsEqual(_player2.State, Player.States.Loser);
            Assert.IsEqual(_player1.State, Player.States.Winner);
        }
        
        
    }
}