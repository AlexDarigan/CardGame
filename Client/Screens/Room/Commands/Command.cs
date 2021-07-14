using System;
using System.Threading.Tasks;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Commands
{
    public abstract class Command
    {
        // Store common operations down here so we can be more declarative in subclasses
        public async Task Execute(Room room)
        {
            room.Effects.RemoveAll();
            Setup(room);
            room.Effects.Start();
            await room.Effects.Executed();
        }

        protected abstract void Setup(Room room);

        protected void Move(Room room, Card card, Zone destination)
        {
            Zone origin = card.CurrentZone;
            origin.Remove(card);
            destination.Add(card);

            const float duration = .2f;
            foreach (Location location in origin.Locations)
            {
                room.Effects.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                room.Effects.InterpolateProperty(location.Card, nameof(Card.RotationDegrees), location.Card.RotationDegrees, location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
			
            foreach (Location location in destination.Locations)
            {
                room.Effects.InterpolateProperty(location.Card, nameof(Card.Translation), location.Card.Translation, location.Translation,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
                
                room.Effects.InterpolateProperty(location.Card, nameof(Card.RotationDegrees), location.Card.RotationDegrees, location.RotationDegrees,
                    duration, Tween.TransitionType.Linear, Tween.EaseType.In);
            }
        }
    }
}