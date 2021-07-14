using System;

namespace CardGame.Client.Commands
{
    public class MoveCard: Command
    {
        // Add MoveType?
        private Who Who { get; }
        private int CardId { get; }
        private SetCodes SetCode { get; }
        private Zones Origin { get; }
        private Zones Destination { get; }
        private int SourceIndex { get; }
        private int DestinationIndex { get; }

        public MoveCard(Who who, int id, SetCodes setCode, Zones origin, Zones destination, int sourceIndex = 0, int destinationIndex = 0)
        {
            Who = who;
            CardId = id;
            SetCode = setCode;
            Origin = origin;
            Destination = destination;
            SourceIndex = sourceIndex;
            DestinationIndex = destinationIndex;
        }
        
        protected override void Setup(Room room)
        {
            // NOTE/TODO: We mean to do this with multiple cards but let's start with one now
            // GetPlayer
            Participant player = Who == Who.Player ? room.Player : room.Rival;
            
            // GetCard
            Card card = Who == Who.Player? room.Cards[CardId]: GetCard(room, player, Origin, SourceIndex, CardId, SetCode);
            
            Zone origin = card.CurrentZone;
            Console.WriteLine(origin.Count);
            Console.WriteLine(card.CurrentZone.Name);

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

        // NOTE: Some Movements don't have a physical representation (Opponent setting a face-down card)..
        // ..but we'll let this execute anyway for the sake of ease (at least until a better alternative is sorted)
        private Card GetCard(Room room, Participant player, Zones from, int at, int id, SetCodes setCodes)
        {
            // We have a card
            Card card = room.Cards[id, setCodes];
            
            Console.WriteLine(from);
            // We have a destination
            Zone origin = from switch
            {
                Zones.Deck => player.Deck,
                Zones.Hand => player.Hand,
                Zones.Supports => player.Supports,
                Zones.Units => player.Units,
                Zones.Discard => player.Discard,
                _ => throw new ArgumentOutOfRangeException()
            };

            // Now we have the location of the copy
            Card copy = origin[at];
            
            // We transfer our card stats to the copy
            copy.Id = card.Id;
            copy.Title = card.Title;
            copy.CardType = card.CardType;
            copy.Art = card.Art;
            copy.Text = card.Text;
            copy.Power = card.Power;
            copy.Faction = card.Faction;
            room.Cards[copy.Id] = copy;
            
            // Delete our unused card (if it isn't our default card)
            // TODO: Investigate if we're creating too many objects here
            if (card.Id != 0) { card.Free(); } ;
            
            // Return a reference to our card that already exists in the scene
            return copy;
        }
    }
}