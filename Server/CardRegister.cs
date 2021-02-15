﻿using System.Collections.Generic;

namespace CardGame.Server
{
    public class CardRegister
    {
        /*
         * Our card register keeps a list of cards in the current game. In the player.LoadDeck function we get
         * the current count of the Cards and set it as the Id of the next card to be added. This means the card id,
         * like a list index, will always be -1 the card count after the card has been added (so if we add a card
         * it will be at index 0 with an id of 0 in a list of count 1)
         */
        private readonly List<Card> Cards = new List<Card>();
        public int Count => Cards.Count;

        public void Add(Card card)
        {
            Cards.Add(card);
        }

        public Card this[int i] => Cards[i];
    }
}