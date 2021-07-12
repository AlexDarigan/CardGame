using System;
using System.Threading.Tasks;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Commands
{
    public abstract class Command
    {
        // Store common operations down here so we can be more declarative in subclasses
        protected int CardId { get; set; } = -1;
        protected SetCodes SetCode { get; set; } = SetCodes.NullCard;
        protected Who Who { get; set; }
        protected Card Card { get; set; }
        protected Participant Player { get; private set; }

        public async Task Execute(Room room)
        {
            room.Effects.RemoveAll();

            Player = Who switch
            {
                Who.Player => room.Player,
                Who.Rival => room.Rival,
                _ => null
            };
            
            // Possible NullReference Exception?
            Card = room.Cards[CardId, SetCode];

            Setup(room);
            room.Effects.Start();
            await room.Effects.Executed();
        }

        protected abstract void Setup(Room room);

        // Helper
        protected void UpdateZone(Room room, Zone zone)
        {
            const float duration = .2f;
            foreach (Location location in zone.Locations)
            {
                room.Effects.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                room.Effects.InterpolateProperty(location.Card, nameof(Card.RotationDegrees), location.Card.RotationDegrees, location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
        }

        protected void MoveCard(Card card, Zone destination, Room room)
        {
            Console.WriteLine($"card is not null: {card is not null}");
            Console.WriteLine($"destination is not null {destination is not null}");
            Zone origin = card.CurrentZone;
            
            origin.Remove(card);
            destination.Add(card);
            UpdateZone(room, origin);
            UpdateZone(room, destination);
        }
    }
}