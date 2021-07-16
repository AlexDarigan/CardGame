namespace CardGame.Client.Commands
{
    public class PlayerSetFaceDown: Command
    {
        private int CardId { get; }

        public PlayerSetFaceDown(int cardId)
        {
            CardId = cardId;
        }

        protected override void Setup(Room room)
        {
            Participant player = room.Player;
            Card card = room.Cards[CardId];
            Move(card, player.Supports);
        }
    }
}