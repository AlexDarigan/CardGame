using System;
using System.Threading.Tasks;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client.Commands
{
    public abstract class Command : Object
    {
        protected Command() { }
        private static int count = 0;
        public async Task Execute(Room room)
        {
            count += 1;
            Console.WriteLine($"{count}: {room.CustomMultiplayer.GetNetworkUniqueId()}: {GetType()}");
            room.Gfx.RemoveAll();
            Setup(room);
            room.Gfx.Start();
            await room.Gfx.ToSignal(room.Gfx, "tween_all_completed");
        }
        
        protected abstract void Setup(Room room);

        // Helper
        protected void UpdateZone(Room room, Zone zone)
        {
            const float duration = .2f;
            foreach (Location location in zone.Locations)
            {
                room.Gfx.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                room.Gfx.InterpolateProperty(location.Card, nameof(Card.RotationDegrees), location.Card.RotationDegrees, location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
        }

        protected void MoveCard(Card card, Zone destination, Room room)
        {
            Zone origin = card.CurrentZone;
            origin.Remove(card);
            destination.Add(card);
            UpdateZone(room, origin);
            UpdateZone(room, destination);
        }
    }
}