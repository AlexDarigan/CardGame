using System;
using System.Threading.Tasks;
using Godot;
using Object = Godot.Object;

namespace CardGame.Client
{
    // Commands are required to be Godot Objects otherwise we can't use .Call()
    public abstract class Command: Object
    {

        protected const string Translation = "translation";
        protected const string RotationDegrees = "rotation_degrees";
        protected Command()
        {
            AddUserSignal("NullCommand");
        }

        public SignalAwaiter Execute(Tween gfx)
        {
            gfx.RemoveAll();
            Setup(gfx);
            gfx.Start();
            return ToSignal(gfx, "tween_all_completed");
        }

        // We don't really need to store the tween info here do we?
        // We could just assign it to base values and remove it afterwards?
        protected abstract void Setup(Tween gfx);
    }
}