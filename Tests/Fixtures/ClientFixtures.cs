using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using CardGame.Client;
using Godot;
using WAT;

namespace CardGame.Tests.Fixtures
{
	[Pre(nameof(Pre))]
	[Post(nameof(Post))]
	public class ClientFixtures : Test
	{
		// NOTE: Make sure we don't time out too early in Yielding
		private readonly PackedScene _mainScene = GD.Load<PackedScene>("res://Main.tscn");
		private Room _room1;
		private Room _room2;
		protected Node GameScene;
		protected Participant P1;
		protected Participant P2;

		// Sometimes the turn player is not Idle, maybe we could add an attribute to the enum values?
		private Participant TurnPlayer => P1.State == States.IdleTurnPlayer ? P1 : P2;

		public async Task Pre()
		{
			GameScene = _mainScene.Instance();
			AddChild(GameScene);
			await Update();
			_room1 = GameScene.GetNode<Room>("Client1/1");
			_room2 = GameScene.GetNode<Room>("Client2/1");
			Console.WriteLine();
			P1 = (Participant) _room1.Get("_player"); //, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_room1);
			P2 = (Participant) _room2.Get("_player"); //, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_room2);
			await Update();
		}

		public void Post()
		{
			GameScene.Free();
		}

		protected async Task Update(float time = 2.5f)
		{
			await UntilSignal(GameScene, nameof(Main.RoomsUpdated), time);
		}
	}
}
