using Godot;

namespace CardGame.Client
{
    public abstract class Command: Object
    {

        // Commands are required to be Godot Objects otherwise we can't use .Call()
        protected Command()
        {
            AddUserSignal("NullCommand");
        }

        public abstract void Execute(Tween gfx);
    }
}