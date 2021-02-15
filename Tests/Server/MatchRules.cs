using System.Collections.Generic;
using CardGame.Server;

namespace CardGame.Tests.Server
{
    public class MatchRules: WAT.Test
    {
        /*
         * We care about the rules in this test. We want to see if our rule checks are working correctly and
         * effectively. While we do care about rules, we can modify some public state on an object to see if a
         * particular check is working. We should only invoke actions that are under test via our game script.
         */
        private void Update()
        {
            
        }

        private readonly List<SetCodes> _deckList = new List<SetCodes>();
        private Player _player1;
        private Player _player2;
        private Match _match;
        private CardRegister _cards;

        public override void Start()
        {
            for (int i = 0; i < 40; i++)
            {
                _deckList.Add(SetCodes.Alpha001);
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
        public void A_Player_Is_Disqualified_When_They_Draw_During_Their_Opponents_Turn()
        {
            _match.Draw(_player2);
            Assert.IsTrue(_player2.Disqualified);
        }
        
        [Test]
        public void A_Player_Is_Disqualified_When_They_Deploy_During_Their_Opponents_Turn()
        {
            
            _match.Deploy(_player2, _player2.Hand[0]);
            Assert.IsTrue(_player2.Disqualified);
        }
    }
}