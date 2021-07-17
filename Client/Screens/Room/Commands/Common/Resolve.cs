using System.Runtime.CompilerServices.Assets.Sounds;

namespace CardGame.Client.Commands
{
    public class Resolve: Command
    {
        protected override void Setup(Room room)
        {
            // There is only one way to resolve a link, from the top down
            room.Effects.InterpolateCallback(room.Link, .25f, nameof(room.Link.Resolve));
            room.Effects.InterpolateCallback(room.SFX, .01f, "set_stream", Sounds.Resolve);
            room.Effects.InterpolateCallback(room.SFX, .25f, "play");
            
        }
    }
}