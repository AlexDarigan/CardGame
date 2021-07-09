using System.Collections;
using System.Linq;

namespace CardGame.Client.Commands
{
    public class LoadDeck : Command
    {
        private IEnumerable DeckList { get; }
        private bool IsPlayer { get; }

        public LoadDeck(bool  isPlayer, IEnumerable deckList)
        {
            IsPlayer = isPlayer;
            DeckList = deckList;
        }

        protected override void Setup(Room room)
        {
            Participant player = room.GetPlayer(IsPlayer);
            foreach (DictionaryEntry pair in DeckList)
            {
                Card card = room.GetCard((int) pair.Key, (SetCodes) pair.Value);
                player.Deck.Add(card);
                card.Controller = player;
                Location location = player.Deck.Locations.Last();
                card.Translation = location.Translation;
                card.RotationDegrees = location.RotationDegrees;
            }
        }
    }
}
