using System;
using System.Collections.Generic;
using CardGame.Server;

namespace CardGame.Tests.Server
{
    public class PlayerAction: WAT.Test
    {
        /*
         * We do not care about rules enforcement in this test. We just want to check what happens when
         * we invoke the primary actions on players to see if the correct state exists. We should typically not invoke
         * actions on our match script in this test (unless it can't otherwise be avoided).
         * NOTE: Since these are action based scripts, we don't care about card type
         */

        private readonly List<SetCodes> _deckList = new List<SetCodes>();
        private Player _player1;
        private Player _player2;
        private Match _match;
        private CardRegister _cards;

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

        private void Update()
        {
            
        }
        
        [Test]
        public void When_A_Player_Draws_A_Card()
        {
            int deckCountBeforeDraw = _player1.Deck.Count;
            int handCountBeforeDraw = _player1.Hand.Count;
            _player1.Draw();
            Assert.IsGreaterThan(deckCountBeforeDraw, _player1.Deck.Count, 
                "Then their deck is reduced in size");
            Assert.IsLessThan(handCountBeforeDraw, _player1.Hand.Count, 
                "Then their hand count is increased in size");
        }
        
        [Test]
        public void When_A_Unit_Is_Deployed()
        {
            int unitCountBeforeDeploy = _player1.Units.Count;
            int handCountBeforeDeploy = _player1.Hand.Count;
            _player1.Deploy(_player1.Hand[0]);
            Assert.IsEqual(_player1.Units.Count, unitCountBeforeDeploy + 1, 
                "Their owners unit count is increased by 1");
            Assert.IsEqual(_player1.Hand.Count, handCountBeforeDeploy - 1,
                "Their owners hand count is reduced by 1");
        }
        
        [Test]
        public void When_A_Card_Is_Set()
        {
            int supportCountBeforeDeploy = _player1.Supports.Count;
            int handCountBeforeDeploy = _player1.Hand.Count;
            _player1.SetFaceDown(_player1.Hand[0]);
            Assert.IsEqual(_player1.Supports.Count, supportCountBeforeDeploy + 1, 
                "Their owners support count is increased by 1");
            Assert.IsEqual(_player1.Hand.Count, handCountBeforeDeploy - 1,
                "Their owners hand count is reduced by 1");
        }
        
        [Test]
        public void When_A_Player_Ends_Their_Turn()
        {
            _match.EndTurn(_player1);
            Assert.IsEqual(_player1.State, Player.States.Passive, "Then their state is passive");
        }
    }
}