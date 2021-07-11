namespace CardGame.Client.Commands
{
    public class DirectAttack: Command
    {
        private int AttackerId { get; }
        
        public DirectAttack(int attackerId) { AttackerId = attackerId; }

        
        protected override void Setup(Room room)
        {
            Card card = room.GetCard(AttackerId);
            if (card.Controller is Player)
            {
                // I now understand y
                //int y = room.RoomView.
               // room.Gfx.InterpolateProperty(AttackerId, nameof(card.Translation), card.Translation);
            }
            else
            {
                
            }
        }
    }
}