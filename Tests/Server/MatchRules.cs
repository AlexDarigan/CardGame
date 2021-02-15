﻿using System.Collections.Generic;
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

        public override string Title()
        {
            return "A Player is Disqualified When";
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
        public void They_Draw_During_Their_Opponents_Turn()
        {
            _match.Draw(_player2);
            Assert.IsTrue(_player2.Disqualified);
        }
        
        [Test]
        public void They_Draw_In_A_NonIdle_State()
        {
            _player1.State = Player.States.Passive;
            _match.Draw(_player1);
            Assert.IsTrue(_player1.Disqualified);
        }
        
        [Test]
        public void They_Deploy_During_Their_Opponents_Turn()
        {
            
            _match.Deploy(_player2, _player2.Hand[0]);
            Assert.IsTrue(_player2.Disqualified);
        }
        
        [Test]
        public void They_Deploy_During_Their_Turn_In_A_NonIdle_State()
        {
            _player1.State = Player.States.Passive;
            _match.Deploy(_player1, _player1.Hand[0]);
            Assert.IsTrue(_player1.Disqualified);
        }
        
        [Test]
        public void They_Deploy_A_NonUnit_Card()
        {
            Card card = _player1.Hand[0];
            card.CardType = CardType.Support;
            _match.Deploy(_player1, card);
            Assert.IsTrue(_player1.Disqualified);
        }
        
        [Test]
        public void They_Set_A_Support_FaceDown_During_Their_Opponents_Turn()
        {
            Card card = _player2.Hand[0];
            card.CardType = CardType.Support;
            _match.SetFaceDown(_player2, card);
            Assert.IsTrue(_player2.Disqualified);
        }
        
        [Test]
        public void They_Set_A_Support_FaceDown_During_Their_Turn_In_A_NonIdle_State()
        {
            Card card = _player1.Hand[0];
            card.CardType = CardType.Support;
            _player1.State = Player.States.Passive;
            _match.SetFaceDown(_player1, card);
            Assert.IsTrue(_player1.Disqualified);
        }

        
        [Test]
        public void They_Set_A_NonSupport_Card_FaceDown()
        {
            Card card = _player1.Hand[0];
            card.CardType = CardType.Unit;
            _match.SetFaceDown(_player1, card);
            Assert.IsTrue(_player1.Disqualified);
        }

        [Test]
        public void They_Declare_An_Attack_When_They_Are_Not_The_Turn_Player()
        {
            Card attacker = _player1.Hand[0];
            Card defender = _player2.Hand[0];
            attacker.CardType = CardType.Unit;
            defender.CardType = CardType.Unit;
            _match.Deploy(_player1, attacker);
            _match.EndTurn(_player1);
            _match.Deploy(_player2, defender);
            _match.EndTurn(_player1);
            _match.DeclareAttack(_player1, attacker, defender);
            Assert.IsTrue(_player1.Disqualified);
        }
        
        [Test]
        public void They_Declare_An_Attack_When_They_Are_In_A_NonIdle_State()
        {
            Card attacker = _player1.Hand[0];
            Card defender = _player2.Hand[0];
            attacker.CardType = CardType.Unit;
            defender.CardType = CardType.Unit;
            _match.Deploy(_player1, attacker);
            _match.EndTurn(_player1);
            _match.Deploy(_player2, defender);
            _match.EndTurn(_player2);
            _player1.State = Player.States.Passive;
            _match.DeclareAttack(_player1, attacker, defender);
            Assert.IsTrue(_player1.Disqualified);
        }
        
        [Test]
        public void They_Declare_An_Attack_With_An_Unready_Unit()
        {
            Card attacker = _player1.Hand[0];
            Card defender = _player2.Hand[0];
            attacker.CardType = CardType.Unit;
            defender.CardType = CardType.Unit;
            _match.EndTurn(_player1);
            _match.Deploy(_player2, defender);
            _match.EndTurn(_player2);
            _match.Deploy(_player1, attacker);
            _match.DeclareAttack(_player1, attacker, defender);
            Assert.IsTrue(_player1.Disqualified);
        }

        [Test]
        public void They_Declare_An_Attack_With_A_Card_That_Is_Not_On_The_Field()
        {
            Card attacker = _player1.Hand[0];
            Card defender = _player2.Hand[0];
            attacker.CardType = CardType.Unit;
            defender.CardType = CardType.Unit;
            _match.EndTurn(_player1);
            _match.Deploy(_player2, defender);
            _match.EndTurn(_player2);
            _match.DeclareAttack(_player1, attacker, defender);
            Assert.IsTrue(_player1.Disqualified);
        }
        
        [Test]
        public void They_Declare_A_Direct_Attack_When_They_Are_Not_The_Turn_Player()
        {
            Card attacker = _player1.Hand[0];
            attacker.CardType = CardType.Unit;
            _match.Deploy(_player1, attacker);
            _match.EndTurn(_player1);
            _match.EndTurn(_player1);
            _match.DeclareDirectAttack(_player1, attacker);
            Assert.IsTrue(_player1.Disqualified);
        }
        
        [Test]
        public void They_Declare_A_Direct_Attack_When_They_Are_In_A_NonIdle_State()
        {
            Card attacker = _player1.Hand[0];
            attacker.CardType = CardType.Unit;
            _match.Deploy(_player1, attacker);
            _match.EndTurn(_player1);
            _match.EndTurn(_player2);
            _player1.State = Player.States.Passive;
            _match.DeclareDirectAttack(_player1, attacker);
            Assert.IsTrue(_player1.Disqualified);
        }
        
        [Test]
        public void They_Declare_A_Direct_Attack_With_An_Unready_Unit()
        {
            Card attacker = _player1.Hand[0];
            attacker.CardType = CardType.Unit;
            _match.EndTurn(_player1);
            _match.EndTurn(_player2);
            _match.Deploy(_player1, attacker);
            _match.DeclareDirectAttack(_player1, attacker);
            Assert.IsTrue(_player1.Disqualified);
        }

        [Test]
        public void They_Declare_A_Direct_Attack_With_A_Card_That_Is_Not_On_The_Field()
        {
            Card attacker = _player1.Hand[0];
            attacker.CardType = CardType.Unit;
            _match.EndTurn(_player1);
            _match.EndTurn(_player2);
            _match.DeclareDirectAttack(_player1, attacker);
            Assert.IsTrue(_player1.Disqualified);
        }

        [Test]
        public void They_Declare_A_Direct_Attack_Against_A_NonEmpty_Field()
        {
            Card attacker = _player1.Hand[0];
            Card defender = _player2.Hand[0];
            attacker.CardType = CardType.Unit;
            defender.CardType = CardType.Unit;
            _match.Deploy(_player1, attacker);
            _match.EndTurn(_player1);
            _match.Deploy(_player2, defender);
            _match.EndTurn(_player2);
            _match.DeclareDirectAttack(_player1, attacker);
            Assert.IsTrue(_player1.Disqualified);
        }
        
        [Test]
        public void They_End_Their_Turn_During_Their_Opponents_Turn()
        {
            _match.EndTurn(_player2);
            Assert.IsTrue(_player2.Disqualified);
        }
        
        [Test]
        public void They_End_Their_Turn_In_A_NonIdle_State()
        {
            _player1.State = Player.States.Passive;
            _match.EndTurn(_player1);
            Assert.IsTrue(_player1.Disqualified);
        }
    }
}