using System.Collections.Generic;
using CardGame.Server;

namespace CardGame.Tests.Server
{
    public class MatchActions: WAT.Test
    {
        /*
         * Our MATCH RULES Test dealt with what would get a player disqualified but we still need to add the positive
         * tests to check against state as a pincer attack. We only need to cover the happy path in these tests
         * so there is to be less tests overall here.
         */
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
            return "A Player is Not Disqualified When";
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
        public void When_A_Player_Draws_A_Card()
        {
            int deckCountBeforeDraw = _player1.Deck.Count;
            int handCountBeforeDraw = _player1.Hand.Count;
            _match.Draw(_player1);
            Assert.IsGreaterThan(deckCountBeforeDraw, _player1.Deck.Count, 
                "Then their deck is reduced in size");
            Assert.IsLessThan(handCountBeforeDraw, _player1.Hand.Count, 
                "Then their hand count is increased in size");
            Assert.IsFalse(_player1.Disqualified);
        }
        
        [Test]
        public void When_A_Unit_Is_Deployed()
        {
            int unitCountBeforeDeploy = _player1.Units.Count;
            int handCountBeforeDeploy = _player1.Hand.Count;
            Card unit = _player1.Hand[0];
            unit.CardType = CardType.Unit;
            _match.Deploy(_player1, unit);
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
            Card support = _player1.Hand[0];
            support.CardType = CardType.Support;
            _match.SetFaceDown(_player1, support);
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