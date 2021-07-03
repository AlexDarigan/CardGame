﻿using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CardGame.Server.Tests
{
    public class ServerFixture: WAT.Test
    {
        // CREATE A DECKBUILDER HELPER
        protected Player P1;
        protected Player P2;
        protected Match Match;

        protected void StartGame(IEnumerable<SetCodes> deckList1 = null, IEnumerable<SetCodes> deckList2 = null)
        {
            P1 = new Player(1, deckList1 ?? BuildDeck());
            P2 = new Player(2,deckList2 ?? BuildDeck());
            Match = new Match(P1, P2, new Cards(), () => {}, (id, command, args) => {});
            Match.Begin(new List<Player> {P1, P2});
        }
        protected static IEnumerable<SetCodes> BuildDeck(SetCodes setCode = SetCodes.NullCard)
        {
            Console.WriteLine(setCode);
            GD.Print(setCode);
            IList<SetCodes> deckList = new List<SetCodes>();
            for (int i = 0; i < 40; i++) { deckList.Add(setCode); }
            return deckList.AsEnumerable();
        }
    }
}