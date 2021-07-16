using System.Collections;
using System.Linq;

namespace CardGame.Client.Commands
{
	public class LoadDeck : Command
	{
		private Who Who { get; }
		private IEnumerable DeckList { get; }

		public LoadDeck(Who who, IEnumerable deckList)
		{
			Who = who;
			DeckList = deckList;
		}

		protected override void Setup(Room room)
		{
			Participant Player = Who == Who.Player ? room.Player : room.Rival;
			foreach (DictionaryEntry pair in DeckList)
			{
				int id = (int) pair.Key;
				SetCodes setCode = (SetCodes) pair.Value;
				room.Cards.Add(id, setCode);
				Card card = room.Cards[id];
				
				Player.Deck.Add(card);
				
				card.Controller = Player;
				// Location location = Player.Deck.Last().Location;
				card.Translation = card.Location.Translation;
				card.RotationDegrees = card.Location.RotationDegrees;
			}
		}
	}
}
