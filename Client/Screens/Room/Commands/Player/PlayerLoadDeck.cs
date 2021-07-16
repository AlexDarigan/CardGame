using System;
using System.Collections;

namespace CardGame.Client.Commands
{
    public class PlayerLoadDeck: Command
    {
        private IEnumerable DeckList { get; }

        public PlayerLoadDeck(IEnumerable deckList)
        {
            DeckList = deckList;
        }
        
        protected override void Setup(Room room)
        {
            Participant player = room.Player;
            foreach (DictionaryEntry pair in DeckList)
            {
                Card card = room.Cards[(int) pair.Key, (SetCodes) pair.Value];
                player.Deck.Add(card);
                card.Controller = player;
                card.Translation = card.Location.Translation;
                card.RotationDegrees = card.Location.RotationDegrees;
            }
        }
    }
}