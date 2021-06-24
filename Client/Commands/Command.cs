using Godot;

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

        public abstract void Execute(Tween gfx);
    }
}