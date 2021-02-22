using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CardGame.Server
{
	public class VirtualStackMachine
	{
		
		private IList<Player> _players;
		private List<Card> _cards;
		private Card _activated;
		private Stack _stack;
		private int _index = 0;
		private int _maxSize;

		public void Activate(Card card)
		{
			_activated = card;
			_stack = new Stack(card.Skill.Instructions.ToList());
			_maxSize = _stack.Count;
			_players = new List<Player> {card.Controller, card.Controller.Opponent};
			_cards = new List<Card>();

			for (_index = 0; _index < _maxSize; _index++)
			{
				Action operation = GetOperation(Next(_index));
				operation();
			}

		}

		private Action GetOperation(Instructions instruction)
		{
			return instruction switch
			{
				// Getters
				Instructions.Literal => Literal,
				Instructions.GetOwningCard => () => _cards.Add(_activated),
				Instructions.GetController => () => Push(0),
				Instructions.GetOpponent => () => Push(1),
				Instructions.GetDeck => () => GetCards(GetPlayer().Deck),
				Instructions.GetGraveyard => () => GetCards(GetPlayer().Graveyard),
				Instructions.GetHand => () => GetCards(GetPlayer().Hand),
				Instructions.GetUnits => () => GetCards(GetPlayer().Units),
				Instructions.GetSupport => () => GetCards(GetPlayer().Supports),
				Instructions.Count => () => Push(_cards.Count),

				// Control Flow
				Instructions.If => If,
				Instructions.GoToEnd => () => _index = _maxSize,

				// Boolean
				Instructions.IsLessThan => () => Compare((a, b) => a < b),
				Instructions.IsGreaterThan => () => Compare((a, b) => a > b),
				Instructions.IsEqual => () => Compare((a, b) => a == b),
				Instructions.IsNotEqual => () => Compare((a, b) => a != b),
				Instructions.And => () => Compare((a, b) => Convert.ToBoolean(a) && Convert.ToBoolean(b)),
				Instructions.Or => () => Compare((a, b) => Convert.ToBoolean(a) && Convert.ToBoolean(b)),

				// Actions
				Instructions.SetFaction => () => SetValue((card, val) => card.Faction = (Faction) val, Pop()),
				Instructions.SetPower => () => SetValue((card, val) => card.Power = val, Pop()),
				Instructions.Destroy => Destroy,
				Instructions.DealDamage => DealDamage,
				Instructions.Draw => Draw,

				// Out of Bounds
				_ => throw new ArgumentOutOfRangeException(nameof(instruction), instruction, "No Valid Operation")
			};
		}

		private int Pop() => _stack.Pop();
		private void Push(int i) => _stack.Push(i);
		private Instructions Next(int i) => (Instructions) _stack[i];
		
		private void Literal()
		{
			_index++;
			_stack.Push(_stack[_index]);
		}
		private void GetCards(IEnumerable<Card> zone) => _cards.AddRange(zone);
		private Player GetPlayer() => _players[Pop()];
		
		private void If()
		{
			// Should Jumps be Implicit?
			const int isTrue = 1;
			int jumpToElseBranch = Pop();
			int success = Pop();
			_index = success == isTrue ? _index : jumpToElseBranch;
		}
		
		private void Compare(Func<int,int, bool> compare)
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = Pop();
			int b = Pop();
			_stack.Push(compare(a, b) ? isTrue : isFalse);
		}

		private void SetValue(Action<Card, int> setter, int value) => _cards.ForEach(card => setter(card, value));
		
		private void Destroy()
		{
			foreach (Card card in _cards)
			{
				card.Zone.Remove(card);
				card.Owner.Graveyard.Add(card);
				card.Zone = card.Owner.Graveyard;
			}
		}

		private void DealDamage()
		{
			Player player = _players[Pop()];
			int damage = Pop();
			player.Health -= damage;
		}
		
		private void Draw()
		{
			Player player = _players[Pop()];
			int count = Pop();
			
			for (int i = 0; i < count; i++)
			{
				Card card = player.Deck[player.Deck.Count - 1];
				player.Deck.Remove(card);
				player.Hand.Add(card);
			}
		}
		
		
		
		










	}
}
