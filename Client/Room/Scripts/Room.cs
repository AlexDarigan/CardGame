using Godot;

namespace CardGame.Client
{
	public class Room: Node
	{
		[Export()]
		private PackedScene Table;
		private const int Server = 1;
		
		public override void _Ready()
		{
			RpcId(Server, "OnClientReady");
			AddChild(Table.Instance());
		}
	}
}
