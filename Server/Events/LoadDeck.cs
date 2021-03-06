﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server.Events
{
    public class LoadDeck : Event
    {
        private Player Controller { get; }
        private Dictionary<int, SetCodes> Deck { get; }

        public LoadDeck(Player controller, Dictionary<int, SetCodes> deck)
        {
            Command = CommandId.LoadDeck;
            Controller = controller;
            Deck = deck;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, Command, Who.Player, Deck.AsEnumerable());
            queue(Controller.Opponent.Id, Command, Who.Rival, NullDeck());
        }

        private static IEnumerable<KeyValuePair<int, SetCodes>> NullDeck()
        {
            Dictionary<int, SetCodes> nullDeck = new();
            for (int i = -1; i > -41; i--) nullDeck[i] = SetCodes.NullCard;

            return nullDeck.AsEnumerable();
        }
    }
}