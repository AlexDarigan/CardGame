using System;
using System.Reflection;
using CardGame.Client;
using Godot;

namespace CardGame.Tests.Client
{
	public class Actions: WAT.Test
	{
		// Note: Must run in Debug otherwise Connections won't work without the TOOL keyword
		// Note: For some reason we can't use mainScene as type Main but as Node it is fine
		// Note: We don't necessarily need to use a connected system to test animations..
		// ..but it might be useful to have both connected and non-connected tests
		private readonly PackedScene MainScene = GD.Load<PackedScene>("res://Main.tscn");
		// DeployAction
		// SetAction
		// ActivateAction
		// EndTurnAction
		// WinAction
		// LoseAction
		// Requires Multiplayer Interface
		// ...Will require an InputInterface eventually

		//[Test]
		public async void RoomsExist()
		{ 
			Node mainScene = MainScene.Instance();
			AddChild(mainScene);
			await ToSignal(UntilSignal(mainScene, nameof(Main.GameBegun), 2.0f), YIELD);
			Assert.IsType<Room>(mainScene.GetNode<Room>("Client1/1"), "Room On Client 1 Exists");
			Assert.IsType<Room>(mainScene.GetNode<Room>("Client2/1"), "Room On Client 2 Exists");
			mainScene.Free();
		}

		[Test]
		public async void DeployAction()
		{
			Node mainScene = MainScene.Instance();
			AddChild(mainScene);
			await ToSignal(UntilSignal(mainScene, nameof(Main.GameBegun), 2.0f), YIELD);
			Room room1 = mainScene.GetNode<Room>("Client1/1");
			Room room2 = mainScene.GetNode<Room>("Client2/1");
			Participant player1 = (Participant) room1.Get("_player");
			await ToSignal(UntilSignal(room1, nameof(Room.Updated), 2.0f), YIELD);
			Assert.IsTrue(player1 is not null, nameof(player1));
			Room room = player1.State == States.IdleTurnPlayer ? room1 : room2;
			Assert.IsEqual(player1.State, States.IdleTurnPlayer, "Player is Idle");
			Assert.IsEqual(player1.Hand.Count, 7);
			Assert.IsEqual(player1.Deck.Count, 33);
			await ToSignal(UntilTimeout(3f), YIELD);
			Card card = player1.Hand[3];
			room.Deploy(card);
			await ToSignal(UntilSignal(room, nameof(Room.Updated), 2.0f), YIELD);
			await ToSignal(UntilTimeout(5.0f), YIELD);
			Assert.IsEqual(player1.Units.Count, 1, "Card was deployed");
			mainScene.Free();
		}
	}
}
