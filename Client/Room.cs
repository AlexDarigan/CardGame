using Godot;

namespace CardGame.Client
{
    public class Room: Node
    {
        private const int Server = 1;
        
        public override void _Ready()
        {
            RpcId(Server, "OnClientReady");
        }
    }
}