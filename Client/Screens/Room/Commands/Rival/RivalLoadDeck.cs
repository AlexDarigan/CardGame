namespace CardGame.Client.Commands
{
    public class RivalLoadDeck: Command
    {
        protected override void Setup(Room room)
        {
            Participant rival = room.Rival;
            for (int id = -1; id > -41; id--)
            {
                Card card = room.Cards[id, SetCodes.NullCard];
                rival.Deck.Add(card);
                card.Controller = rival;
                card.Translation = card.Location.Translation;
                card.RotationDegrees = card.Location.RotationDegrees;
            }
        }
    }
}
