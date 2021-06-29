using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGame.Client;
using CardGame.Tests.Fixtures;
using Godot;

namespace CardGame.Tests.Client
{
	public class Actions: ClientFixtures //WAT.Test //CardGame.Tests.Client.Fixture
	{
		// Note: Must run in Debug otherwise Connections won't work without the TOOL keyword
		// Note: For some reason we can't use mainScene as type Main but as Node it is fine
		// Note: We don't necessarily need to use a connected system to test animations..
		// ..but it might be useful to have both connected and non-connected tests
		// DeployAction
		// SetAction
		// ActivateAction
		// EndTurnAction
		// WinAction
		// LoseAction
		// Requires Multiplayer Interface
		// ...Will require an InputInterface eventually

		[Test]
		public void RoomsExist()
		{
			Assert.IsType<Room>(GameScene.GetNode<Room>("Client1/1"), "Room On Client 1 Exists");
			Assert.IsType<Room>(GameScene.GetNode<Room>("Client2/1"), "Room On Client 2 Exists");
		}
		
		[Test]
		public async Task OnGameStart()
		{
			await Update();
			Assert.IsEqual(P1.State, States.IdleTurnPlayer, "Player 1 is Idle Turn Player");
			Assert.IsEqual(P2.State, States.Passive, "Player 2 is Passive");
			Assert.IsEqual(P1.Hand.Count, 7, "Player 1 hand count is 7");
			Assert.IsEqual(P2.Hand.Count, 7, "Player 2 hand count is 7");
			Assert.IsEqual(P1.Deck.Count, 33, "Player 1 deck count is 33");
			Assert.IsEqual(P2.Deck.Count, 33, "Player 2 deck count is 33");
		}

		[Test]
		public async Task DeployAction()
		{
			Card card = P1.Hand.ToList().Where(c => (CardState) c.Get("CardState") == CardState.Deploy).ElementAt(0);
			P1.OnCardPressed(card);
			await Update();
			Assert.IsEqual(P1.Units.Count, 1, "Card was deployed");
		}
		
		[Test]
		public async Task SetAction()
		{
			// We'll have to inject decks into here somewhere
			Card card = P1.Hand.ToList().Where(c => (CardState) c.Get("CardState") == CardState.Set).ElementAt(0);
			P1.OnCardPressed(card);
			await Update();
			Assert.IsEqual(P1.Support.Count, 1, "Card was set");
		}

		
	}
}
