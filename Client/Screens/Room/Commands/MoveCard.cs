using System;

namespace CardGame.Client.Commands
{
    public class MoveCard: Command
    {
        // Add MoveType?
        private Who Who { get; }
        private int CardId { get; }
        private SetCodes SetCode { get; }
        private Zones Destination { get; }

        public MoveCard(Who who, int id, SetCodes setCode, Zones destination)
        {
            Who = who;
            CardId = id;
            SetCode = setCode;
            Destination = destination;
        }
        
        protected override void Setup(Room room)
        {
            // NOTE/TODO: We mean to do this with multiple cards but let's start with one now
            // GetPlayer
            Participant player = Who == Who.Player ? room.Player : room.Rival;
            
            // GetCard
            Card card = room.Cards[CardId, SetCode];
                // If Opponent, SwapCards (Let's focus on player movement right now)
                // We would use an index to fetch the card at the current pos, that switch the stats through Cards Node
            // Get Origin
            Zone origin = card.CurrentZone;

            // Get Destination
            Zone destination = Destination switch
            {
                Zones.Deck => player.Deck,
                Zones.Hand => player.Hand,
                Zones.Supports => player.Supports,
                Zones.Units => player.Units,
                Zones.Discard => player.Discard,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            Move(room, card, destination);
        }
    }
}