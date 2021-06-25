using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CardGame.Client;
using Godot;


namespace CardGame.Server
{
	public delegate void Enqueue(int id, CommandId command, params object[] args);

	public class Room: Node
	{
		private readonly Match _match;
		private readonly System.Collections.Generic.Dictionary<int, Player> _players = new System.Collections.Generic.Dictionary<int, Player>();
		private readonly CardRegister _cards = new CardRegister();
	 
		public Room()
		{ 
			// I believe an empty constructor is required in Godot Classes that have non-empty constructor(s)
			// ..for the sake of some Godot callbacks
			
		}
		public Room(Player player1, Player player2)
		{
			_players[player1.Id] = player1;
			_players[player2.Id] = player2;
			_match = new Match(player1, player2, _cards, Update, Queue);
		}

		private void Update()
		{
			// Update Card/Player State Information
			// Execute Queued Events
			foreach (Card card in _cards)
			{
				//card.Update(() => {});
			}
			foreach (int id in _players.Keys)
			{
				RpcId(id, "SetState", _players[id].State);
				RpcId(id, "Update");
			}
		}

		private void Queue(int player, CommandId commandId, params object[] args)
		{
			RpcId(player, "Queue", commandId, args);
		}
		

		[Master]
		public void OnClientReady()
		{
			_players[CustomMultiplayer.GetRpcSenderId()].Ready = true;
			if (_players.Values.Any(player => !player.Ready))
			{
				return;
			}
			_match.Begin(_players.Values.ToList());

			
			// Since all of our work is done in the constructor we will be ready to push our events here
			// (With that in mind, should we queue everything serverside and only push it once when ready to update?..
			// ..arguably that could help with some peekAhead methods on client-side for better queueing).
			Update();
		}

		[Master]
		public void Deploy(int cardId)
		{
			_match.Deploy(_players[Multiplayer.GetRpcSenderId()], _cards[cardId]);
		}

		[Master]
		public void EndTurn(int playerId)
		{
			_match.EndTurn(_players[playerId]);
		}
	}
}
