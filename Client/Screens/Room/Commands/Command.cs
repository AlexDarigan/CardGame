using System;
using System.Threading.Tasks;
using Godot;
using JetBrains.Annotations;
using Object = Godot.Object;

namespace CardGame.Client.Commands
{
    public abstract class Command: Object
    {
        protected Room Room;
        // Store common operations down here so we can be more declarative in subclasses
        public async Task Execute(Room room)
        {
            Room = room;
            room.Effects.RemoveAll();
            Setup(room);
            Room = null;
            room.Effects.Start();
            await ToSignal(room.Effects, "tween_all_completed");
        }

        protected abstract void Setup(Room room);
    }
}