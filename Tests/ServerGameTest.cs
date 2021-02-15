﻿using System.Collections.Generic;
using System.Linq;
using CardGame.Server;
using WAT;

namespace CardGame.Tests
{
    public class ServerGameTest: WAT.Test
    {
        private readonly List<SetCodes> DeckList = new List<SetCodes>();
        private Player _player1;
        private Player _player2;
        private Room _room;
        
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
            _room = new Room(_player1, _player2);
            _room.Start();
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
        public void And_A_Player_Draws()
        {
            Player player1 = new Player(1, DeckList);
            Player player2 = new Player(2, DeckList);
            Room room = new Room(player1, player2);
            room.Start();
            int deckCountBeforeDraw = player1.Deck.Count;
            int handCountBeforeDraw = player1.Hand.Count;
            player1.Draw();
            Assert.IsGreaterThan(deckCountBeforeDraw, player1.Deck.Count, 
                "Then their deck is reduced in size");
            Assert.IsLessThan(handCountBeforeDraw, player1.Hand.Count, 
                "Then their hand count is increased in size");
            room.Free();
        }

        [Test]
        public void Deck_Contents_As_SetCodes_Equal_DeckList_SetCodes()
        {
            bool success = _player1.Deck.All(card => card.SetCodes == SetCodes.Alpha001) &&
                           _player1.Hand.All(card => card.SetCodes == SetCodes.Alpha001);
            Assert.IsTrue(success);
        }
        

        public override void End()
        {
            _room.Free();
        }
    }
}