using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Commands
{
    public abstract class Command
    {
        public bool IsPlayer { get; set; }
        public Participant Player { get; set; }
        
        public async Task Execute(Room room)
        {
            room.Effects.RemoveAll();
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
            Zone origin = card.CurrentZone;
            origin.Remove(card);
            destination.Add(card);
            UpdateZone(room, origin);
            UpdateZone(room, destination);
        }
    }
}