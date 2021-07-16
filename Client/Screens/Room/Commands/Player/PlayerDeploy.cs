namespace CardGame.Client.Commands
{
    public class PlayerDeploy: Command
    {
        private int CardId { get; }

        public PlayerDeploy(int cardId)
        {
            CardId = cardId;
        }


        protected override void Setup(Room room)
        {
            Participant player = room.Player;
            Card card = room.Cards[CardId];
            Move(card, player.Units);
        }
    }
}