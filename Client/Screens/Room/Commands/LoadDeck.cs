using System.Collections;
using System.Linq;

namespace CardGame.Client.Commands
{
	public class LoadDeck : Command
	{
		private IEnumerable DeckList { get; }

		public LoadDeck(Who who, IEnumerable deckList)
		{
			Who = who;
			DeckList = deckList;
		}

		protected override void Setup(Room room)
		{
			foreach (DictionaryEntry pair in DeckList)
			{
				int id = (int) pair.Key;
				SetCodes setCode = (SetCodes) pair.Value;
				room.Cards.Add(id, setCode);
				Card card = room.Cards[id];
				
				Player.Deck.Add(card);
				
				card.Controller = Player;
				Location location = Player.Deck.Locations.Last();
				card.Translation = location.Translation;
				card.RotationDegrees = location.RotationDegrees;
			}
		}
	}
}
