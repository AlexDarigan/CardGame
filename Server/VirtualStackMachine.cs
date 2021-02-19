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
		private Stack _instructions;
		private int _index = 0;
		private int _maxSize;

		public void Activate(Card card)
		{
			_activated = card;
			_instructions = new Stack(card.Skill.Instructions.ToList());
			_maxSize = _instructions.Count;
			_players = new List<Player> {card.Controller, card.Controller.Opponent};
			_cards = new List<Card>();

			for (_index = 0; _index < _maxSize; _index++)
			{
				int instruction = _instructions[_index];
				Action operation = GetOperation((Instructions) instruction);
				operation();
			}

		}

		private Action GetOperation(Instructions instruction)
		{
			return instruction switch
			{
				Instructions.Draw => Draw,
				Instructions.Literal => Literal,
				Instructions.GetOwningCard => GetOwningCard,
				Instructions.GetController => GetController,
				Instructions.GetOpponent => GetOpponent,
				Instructions.GetDeck => GetDeck,
				Instructions.GetGraveyard => GetGraveyard,
				Instructions.GetHand => GetHand,
				Instructions.GetUnits => GetUnits,
				Instructions.GetSupport => GetSupports,
				Instructions.GoToEnd => GoToEnd,
				Instructions.Count => Count,
				Instructions.IsLessThan => IsLessThan,
				Instructions.IsGreaterThan => IsGreaterThan,
				Instructions.IsEqual => IsEqual,
				Instructions.IsNotEqual => IsNotEqual,
				Instructions.If => If,
				Instructions.And => And,
				Instructions.Or => Or,
				Instructions.SetFaction => SetFaction,
				Instructions.SetPower => SetPower,
				Instructions.Destroy => Destroy,
				Instructions.DealDamage => DealDamage,
				_ => throw new ArgumentOutOfRangeException(nameof(instruction), instruction, null)
			};
			
		}

		private void Draw()
		{
			Player player = _players[_instructions.Pop()];
			int count = _instructions.Pop();
			
			for (int i = 0; i < count; i++)
			{
				Card card = player.Deck[player.Deck.Count - 1];
				player.Deck.Remove(card);
				player.Hand.Add(card);
			}
		}

		private void DealDamage()
		{
			Player player = _players[_instructions.Pop()];
			int damage = _instructions.Pop();
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
			_instructions.Push(_instructions[_index]);
		}

		private void IsGreaterThan()
		{
			const int isFalse = 0;
            const int isTrue = 1;
            int a = _instructions.Pop();
            int b = _instructions.Pop();
            _instructions.Push(a > b ? isTrue : isFalse);
		}
		
		private void IsLessThan()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = _instructions.Pop();
			int b = _instructions.Pop();
			_instructions.Push(a < b ? isTrue : isFalse);
		}

		private void IsEqual()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = _instructions.Pop();
			int b = _instructions.Pop();
			_instructions.Push(a == b ? isTrue : isFalse);
		}

		private void IsNotEqual()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = _instructions.Pop();
			int b = _instructions.Pop();
			_instructions.Push(a != b ? isTrue : isFalse);
		}

		private void If()
		{
        	const int isTrue = 1;
        	// Should Jumps be Implicit?
        	int jump = _instructions.Pop();
        	int success = _instructions.Pop();
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
			int a = _instructions.Pop();
			int b = _instructions.Pop();
			_instructions.Push(a == isTrue && b == isTrue ? isTrue : isFalse);
		}

		private void Or()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = _instructions.Pop();
			int b = _instructions.Pop();
			_instructions.Push(a == isTrue || b == isTrue ? isTrue : isFalse);
		}
		
		private void SetFaction()
		{
			Faction faction = (Faction) _instructions.Pop();
			foreach (Card card in _cards) { card.Faction = faction; }
		}
		
		private void SetPower()
		{
			int power = _instructions.Pop();
			foreach (Card card in _cards) { card.Power = power; }
		}

		private void GoToEnd()
		{
			_index = _maxSize;
		}

		private void Count()
		{
			_instructions.Push(_cards.Count);
		}

		private void GetController()
		{
			_instructions.Push(0);
		}
		
		private void GetOpponent()
		{
			_instructions.Push(1);
		}

		private void GetDeck()
		{
			_cards.AddRange(_players[_instructions.Pop()].Deck);
		}
		
		private void GetHand()
		{
			_cards.AddRange(_players[_instructions.Pop()].Hand);
		}
		
		private void GetUnits()
		{
			_cards.AddRange(_players[_instructions.Pop()].Units);
		}
		
		private void GetSupports()
		{
			_cards.AddRange(_players[_instructions.Pop()].Supports);
		}
		
		private void GetGraveyard()
		{
			_cards.AddRange(_players[_instructions.Pop()].Graveyard);
		}

		private void GetOwningCard()
		{
			// All Cards are stored in a list even if they're individual in case a further..
			// ..instruction requires to group a number of them together
			// instructions.Push(new List<Card>{card});
			_cards.Add(_activated);
		}
		
		
		

		

	}
}
