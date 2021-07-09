using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Commands
{
    public abstract class Command : Object
    {
        protected Command() { }

        public async Task Execute(CommandQueue gfx)
        {
            gfx.RemoveAll();
            Setup(gfx);
            gfx.Start();
            await gfx.ToSignal(gfx, "tween_all_completed");
        }
        
        protected abstract void Setup(CommandQueue gfx);

        // Helper
        protected void UpdateZone(CommandQueue gfx, Zone zone)
        {
            const float duration = .2f;
            foreach (Location location in zone.Locations)
            {
                gfx.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                gfx.InterpolateProperty(location.Card, nameof(Card.RotationDegrees), location.Card.RotationDegrees, location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
        }

        protected void MoveCard(Card card, Zone destination, CommandQueue gfx)
        {
            Zone origin = card.CurrentZone;
            origin.Remove(card);
            destination.Add(card);
            UpdateZone(gfx, origin);
            UpdateZone(gfx, destination);
        }
    }
}