using System.Threading.Tasks;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Commands
{
    // Commands are required to be Godot Objects otherwise we can't use .Call()
    public abstract class Command : Object
    {
        
        public Command()
        {
            AddUserSignal("NullCommand");
        }

        public async Task Execute(CommandQueue gfx)
        {
            gfx.RemoveAll();
            Setup(gfx);
            gfx.Start();
            await ToSignal(gfx, "tween_all_completed");
        }

        // We don't really need to store the tween info here do we?
        // We could just assign it to base values and remove it afterwards?
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

        protected void MoveCard(Card card, Zone origin, Zone destination, CommandQueue gfx)
        {
            origin.Remove(card);
            destination.Add(card);
            UpdateZone(gfx, origin);
            UpdateZone(gfx, destination);
        }
    }
}