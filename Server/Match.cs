using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Godot;
using Newtonsoft.Json.Converters;

namespace CardGame.Server
{
	public class Match
	{
		/*
		 * Match is a Rules-Enforced Script. This means any action called from here will have
		 * to be a legal play otherwise the player invoking the illegal play will be disqualified.
		 * When we want to test actions independently of a script like this, we will just invoke them
		 * directly on their owners (either a player or a skill).
		 */

		private readonly VirtualStackMachine _virtualStackMachine = new VirtualStackMachine();
		// May be an idea
		private readonly Action Update;
		private readonly Enqueue Queue;
		private bool _isGameOver;
		private readonly CardRegister CardRegister;
		public Match(Player player1, Player player2, CardRegister cardRegister, Action update, Enqueue queue)
		{
			Update = update;
			Queue = queue;
			CardRegister = cardRegister;
			player1.Opponent = player2;
			player2.Opponent = player1;
		}

		public void Begin(List<Player> players)
		{
			// Could place this directly inside room (we'd remove the register dependency for a start)
			foreach (Player player in players)
			{
				player.LoadDeck(CardRegister).QueueOnClients(Queue);
				for (int i = 0; i < 7; i++)
				{
					player.Draw().QueueOnClients(Queue);
				}
			}

			players[0].State = States.IdleTurnPlayer;
		}
		
		public void Draw(Player player)
		{
			if (player.State != States.IdleTurnPlayer)
			{
				Console.WriteLine($"{player} state is {player.State.ToString()}");
				Disqualify(player);
				return;
			}

			if (player.Deck.Count == 0)
			{
				GameOver(player.Opponent, player);
				return;
			}
			player.Draw();
			Update();
		}

		public void Deploy(Player player, Card unit)
		{
			if (unit.CardState != CardState.Deploy)
			{
				Disqualify(player);
				return;
			}
			
			Console.WriteLine("Deploying?");
			player.Deploy(unit).QueueOnClients(Queue);
			Update();
		}

		public void DeclareAttack(Player player, Card attacker, Card defender)
		{
			if (attacker.CardState != CardState.AttackUnit)
			{
				Disqualify(player);
				return;
			}
			else
			{
				Console.WriteLine("Can Attack Unit");
			}

			static void DamageCalculation(Card winner, Card loser)
			{
				loser.Controller.Health -= winner.Power - loser.Power;
				loser.Controller.Units.Remove(loser);
				loser.Owner.Graveyard.Add(loser);
			}

			if (attacker.Power > defender.Power)
			{
				DamageCalculation(attacker, defender);
			}
			else if(defender.Power > attacker.Power)
			{
				DamageCalculation(defender, attacker);
			}

			Update();
		}
		
		public void DeclareDirectAttack(Player player, Card attacker)
		{
			if (attacker.CardState != CardState.AttackPlayer)
			{
				Disqualify(player);
				return;
			}

			player.Opponent.Health -= attacker.Power;
			Update();
		}

		public void SetFaceDown(Player player, Card support)
		{
			Console.WriteLine("Setting FaceDown");
			if (support.CardState != CardState.Set)
			{
				Console.WriteLine("Disqualified");
				Console.WriteLine(support.CardType);
				Disqualify(player);
				return;
			}

			player.SetFaceDown(support).QueueOnClients(Queue);
			Update();
		}

		public void Activate(Player player, Card support)
		{
			if (player.State != States.IdleTurnPlayer || !player.Supports.Contains(support))
			{
				Disqualify(player);
				return;
			}
			
			_virtualStackMachine.Activate(support);
			Update();
		}
		
		
		public void EndTurn(Player player)
		{
			if (player.State != States.IdleTurnPlayer)
			{
				Disqualify(player);
				return;
			}
			
			player.State = States.Passive;
			player.Opponent.State = States.IdleTurnPlayer;
			Draw(player.Opponent);
			foreach (Card card in player.Units) { card.IsReady = true; }
			foreach (Card card in player.Supports) { card.IsReady = true; }
			Update();
		}

		private void Disqualify(Player player)
		{
			if (_isGameOver)
			{
				// The Player States are already invalid at this point so no reason to force the disqualification
				return;
			}
			player.Disqualified = true;
			Debug.WriteLine($"Disqualified {player}");
		}

		private void GameOver(Player winner, Player loser)
		{
			winner.State = States.Winner;
			loser.State = States.Loser;
			_isGameOver = true;
			Update();
		}
	}
}
