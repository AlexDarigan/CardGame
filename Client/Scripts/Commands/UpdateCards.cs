using System;
using System.Collections;

namespace CardGame.Client.Commands
{
    public class UpdateCards: Command
    {
        private IEnumerable Cards { get; }

        public UpdateCards(IEnumerable cards) { Cards = cards; }
        
        protected override void Setup(Room room)
        {
            foreach (DictionaryEntry pair in Cards) { room.GetCard((int) pair.Key).CardState.Set((CardStates) pair.Value); }
        }
    }
}