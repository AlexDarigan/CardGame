using System;
using System.Collections;
using System.Collections.Generic;

namespace CardGame.Server
{
    public class Cards : IEnumerable<Card>
    {
        /*
         * Our card register keeps a list of cards in the current game. In the player.LoadDeck function we get
         * the current count of the Cards and set it as the Id of the next card to be added. This means the card id,
         * like a list index, will always be -1 the card count after the card has been added (so if we add a card
         * it will be at index 0 with an id of 0 in a list of count 1)
         */
        private readonly List<Card> _cards = new();
        public int Count => _cards.Count;
        public Card this[int i] => _cards[i];
        public Card CreateCard(SetCodes setCodes, Player owner)
        {
            CardInfo cardInfo = Library.Cards[setCodes];
            Card card = new Card(_cards.Count, owner)
            {
                SetCodes = setCodes,
                Title = cardInfo.Title,
                CardType = cardInfo.CardType,
                Faction = cardInfo.Faction,
                Power = cardInfo.Power
            };
            SkillInfo skillInfo = cardInfo.Skill;
            card.Skill = new Skill(card, skillInfo.Triggers, skillInfo.Instructions, skillInfo.Description);
            _cards.Add(card);
            return card;   
        }

        public IEnumerator<Card> GetEnumerator() => _cards.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
