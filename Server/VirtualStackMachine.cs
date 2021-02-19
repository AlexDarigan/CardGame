using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;

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
				int instruction = _stack[_index];
				Action operation = GetOperation((Instructions) instruction);
				operation();
			}

		}

		private Action GetOperation(Instructions instruction) => instruction switch
		{
			// Getters
			Instructions.Literal => Literal,
			Instructions.GetOwningCard => GetOwningCard,
			Instructions.GetController => GetController,
			Instructions.GetOpponent => GetOpponent,
			Instructions.GetDeck => GetDeck,
			Instructions.GetGraveyard => GetGraveyard,
			Instructions.GetHand => GetHand,
			Instructions.GetUnits => GetUnits,
			Instructions.GetSupport => GetSupports,
			Instructions.Count => Count,
			
			// Control Flow
			Instructions.If => If,
			Instructions.GoToEnd => GoToEnd,

			// Boolean
			Instructions.IsLessThan => IsLessThan,
			Instructions.IsGreaterThan => IsGreaterThan,
			Instructions.IsEqual => IsEqual,
			Instructions.IsNotEqual => IsNotEqual,
			Instructions.And => And,
			Instructions.Or => Or,
			
			// Actions
			Instructions.SetFaction => SetFaction,
			Instructions.SetPower => SetPower,
			Instructions.Destroy => Destroy,
			Instructions.DealDamage => DealDamage,
			Instructions.Draw => Draw,
			
			// Out of Bounds
			_ => throw new ArgumentOutOfRangeException(nameof(instruction), instruction, "No Valid Operation")
		};

		private void Draw()
		{
			Player player = _players[_stack.Pop()];
			int count = _stack.Pop();
			
			for (int i = 0; i < count; i++)
			{
				Card card = player.Deck[player.Deck.Count - 1];
				player.Deck.Remove(card);
				player.Hand.Add(card);
			}
		}

		private void DealDamage()
		{
			Player player = _players[_stack.Pop()];
			int damage = _stack.Pop();
			player.Health -= damage;
		}
		

		private void Destroy()
		{
			foreach (Card card in _cards)
			{
				card.Zone.Remove(card);
				card.Owner.Graveyard.Add(card);
				card.Zone = card.Owner.Graveyard;
			}
		}

		private void Literal()
		{
			_index++;
			_stack.Push(_stack[_index]);
		}

		private void IsGreaterThan()
		{
			const int isFalse = 0;
            const int isTrue = 1;
            int a = _stack.Pop();
            int b = _stack.Pop();
            _stack.Push(a > b ? isTrue : isFalse);
		}
		
		private void IsLessThan()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = _stack.Pop();
			int b = _stack.Pop();
			_stack.Push(a < b ? isTrue : isFalse);
		}

		private void IsEqual()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = _stack.Pop();
			int b = _stack.Pop();
			_stack.Push(a == b ? isTrue : isFalse);
		}

		private void IsNotEqual()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = _stack.Pop();
			int b = _stack.Pop();
			_stack.Push(a != b ? isTrue : isFalse);
		}

		private void If()
		{
        	const int isTrue = 1;
        	// Should Jumps be Implicit?
        	int jump = _stack.Pop();
        	int success = _stack.Pop();
        	if (success == isTrue)
        	{
        		
        	}
        	else
        	{
        		_index = jump;
        	}
        }
		
		private void And()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = _stack.Pop();
			int b = _stack.Pop();
			_stack.Push(a == isTrue && b == isTrue ? isTrue : isFalse);
		}

		private void Or()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = _stack.Pop();
			int b = _stack.Pop();
			_stack.Push(a == isTrue || b == isTrue ? isTrue : isFalse);
		}
		
		private void SetFaction()
		{
			Faction faction = (Faction) _stack.Pop();
			foreach (Card card in _cards) { card.Faction = faction; }
		}
		
		private void SetPower()
		{
			int power = _stack.Pop();
			foreach (Card card in _cards) { card.Power = power; }
		}

		private void GoToEnd() => _index = _maxSize;
		private void Count() => _stack.Push(_cards.Count);
		private void GetController() => _stack.Push(0);
		private void GetOpponent() => _stack.Push(1);
		private void GetDeck() => _cards.AddRange(_players[_stack.Pop()].Deck);
		private void GetHand() => _cards.AddRange(_players[_stack.Pop()].Hand);
		private void GetUnits() =>	_cards.AddRange(_players[_stack.Pop()].Units);
		private void GetSupports() => _cards.AddRange(_players[_stack.Pop()].Supports);
		private void GetGraveyard() => _cards.AddRange(_players[_stack.Pop()].Graveyard);
		private void GetOwningCard() => _cards.Add(_activated);






	}
}
