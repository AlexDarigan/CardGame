using System;
using System.Diagnostics;

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
        // Change From Card to Cards and unpack an IEnumerable

        public MoveCard(Who who, int id, SetCodes setCode, Zones origin, Zones destination, int sourceIndex, int destinationIndex)
        {
            Who = who;
            CardId = id;
            SetCode = setCode;
            Origin = origin;
            Destination = destination;
            SourceIndex = sourceIndex;
            DestinationIndex = destinationIndex;
            Console.WriteLine($"Source Value At Client Command is {SourceIndex}");
        }
        
        protected override void Setup(Room room)
        {
            // TODO: Implement with multiple players
            Participant player = Who == Who.Player ? room.Player : room.Rival;
            Card card = Who == Who.Player? room.Cards[CardId]: GetCard(room, player, Origin, SourceIndex, CardId, SetCode);
            
            Zone origin = card.CurrentZone;
            
            // Get Destination
            Zone destination = Destination switch
            {
                Zones.Deck => player.Deck,
                Zones.Hand => player.Hand,
                Zones.Supports => player.Supports,
                Zones.Units => player.Units,
                Zones.Discard => player.Graveyard,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            Console.WriteLine(destination.Name);
            Move(room, card, destination);
        }

        // NOTE: Some Movements don't have a physical representation (Opponent setting a face-down card)..
        // ..but we'll let this execute anyway for the sake of ease (at least until a better alternative is sorted)
        private Card GetCard(Room room, Participant player, Zones from, int at, int id, SetCodes setCodes)
        {
            // Figure out where we're moving our card from
            Zone origin = from switch
            {
                Zones.Deck => player.Deck,
                Zones.Hand => player.Hand,
                Zones.Supports => player.Supports,
                Zones.Units => player.Units,
                Zones.Discard => player.Graveyard,
                _ => throw new ArgumentOutOfRangeException()
            };

           
            // Now we have the location of the copy
            Console.WriteLine(origin.Name);
            Card card = origin[at];
            //return card;
            // // Card is not revealed to us so we just use the blank element
            if (SetCode == SetCodes.NullCard) { return card; }
            
            // Card is known to us so we transfer our archived data to the actual object
            Card prototype = room.Cards[id, setCodes];
            
            if (prototype == card)
            {
                // No need to transfer data between card A and card A
                return card;
            }
            
            // Transfer data from our new card (prototype) to the blank card that already existed in origin
            card.Id = prototype.Id;
            card.Title = prototype.Title;
            card.CardType = prototype.CardType;
            card.Art = prototype.Art;
            card.Text = prototype.Text;
            card.Power = prototype.Power;
            card.Faction = prototype.Faction;
            
            // Card has taken over the role of the newly created card
            room.Cards[card.Id] = card;
            prototype.Free(); // TODO: Fix Null Reference Exception (QueueFree won't cut it)
            return card;
        }
    }
}