using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Commands
{
    public abstract class Command
    {
        // Store common operations down here so we can be more declarative in subclasses
        protected int CardId { get; set; }
        protected SetCodes SetCode { get; set; }
        protected bool IsPlayer { get; set; }
        protected Card Card => Room.GetCard(CardId, SetCode);
        public Participant Player => Room.GetPlayer(IsPlayer);
        private Room Room { get; set; }
        
        public async Task Execute(Room room)
        {
            Room = room;
            Room.Effects.RemoveAll();
            Setup(Room);
            Room.Effects.Start();
            await Room.Effects.Executed();
            room = null;
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