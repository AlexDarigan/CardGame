using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;

namespace CardGame.Server
{
	public class VirtualStackMachine
	{
		private int index = 0;
		private Stack instructions;
		private IList<Player> players;
		List<Card> cards;
		private Card Activated;
		private int maxSize;

		public void Activate(Card card)
		{
			Activated = card;
			maxSize = card.Skill.Instructions.Count();
			instructions = new Stack(card.Skill.Instructions.ToList());
			players = new List<Player> {card.Controller, card.Controller.Opponent};
			cards = new List<Card>();

			for (index = 0; index < maxSize; index++)
			{
				Instructions instruction = (Instructions) instructions[index];
				Action operation = GetOperation(instruction);
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
			Player player = players[instructions.Pop()];
			int count = instructions.Pop();
			
			for (int i = 0; i < count; i++)
			{
				Card card = player.Deck[player.Deck.Count - 1];
				player.Deck.Remove(card);
				player.Hand.Add(card);
			}
		}

		private void DealDamage()
		{
			Player player = players[instructions.Pop()];
			int damage = instructions.Pop();
			player.Health -= damage;
		}
		

		private void Destroy()
		{
			foreach (Card card in cards)
			{
				card.Zone.Remove(card);
				card.Owner.Graveyard.Add(card);
				card.Zone = card.Owner.Graveyard;
			}
		}

		private void Literal()
		{
			index++;
			instructions.Push(instructions[index]);
		}

		private void IsGreaterThan()
		{
			const int isFalse = 0;
            const int isTrue = 1;
            int a = instructions.Pop();
            int b = instructions.Pop();
            instructions.Push(a > b ? isTrue : isFalse);
		}
		
		private void IsLessThan()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = instructions.Pop();
			int b = instructions.Pop();
			instructions.Push(a < b ? isTrue : isFalse);
		}

		private void IsEqual()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = instructions.Pop();
			int b = instructions.Pop();
			instructions.Push(a == b ? isTrue : isFalse);
		}

		private void IsNotEqual()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = instructions.Pop();
			int b = instructions.Pop();
			instructions.Push(a != b ? isTrue : isFalse);
		}

		private void If()
		{
        	const int isTrue = 1;
        	// Should Jumps be Implicit?
        	int jump = instructions.Pop();
        	int success = instructions.Pop();
        	if (success == isTrue)
        	{
        		
        	}
        	else
        	{
        		index = jump;
        	}
        }
		
		private void And()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = instructions.Pop();
			int b = instructions.Pop();
			instructions.Push(a == isTrue && b == isTrue ? isTrue : isFalse);
		}

		private void Or()
		{
			const int isFalse = 0;
			const int isTrue = 1;
			int a = instructions.Pop();
			int b = instructions.Pop();
			instructions.Push(a == isTrue || b == isTrue ? isTrue : isFalse);
		}
		
		private void SetFaction()
		{
			Faction faction = (Faction) instructions.Pop();
			foreach (Card card in cards) { card.Faction = faction; }
		}
		
		private void SetPower()
		{
			int power = instructions.Pop();
			foreach (Card card in cards) { card.Power = power; }
		}

		private void GoToEnd()
		{
			index = maxSize;
		}

		private void Count()
		{
			instructions.Push(cards.Count);
		}

		private void GetController()
		{
			instructions.Push(0);
		}
		
		private void GetOpponent()
		{
			instructions.Push(1);
		}

		private void GetDeck()
		{
			cards.AddRange(players[instructions.Pop()].Deck);
		}
		
		private void GetHand()
		{
			cards.AddRange(players[instructions.Pop()].Hand);
		}
		
		private void GetUnits()
		{
			cards.AddRange(players[instructions.Pop()].Units);
		}
		
		private void GetSupports()
		{
			cards.AddRange(players[instructions.Pop()].Supports);
		}
		
		private void GetGraveyard()
		{
			cards.AddRange(players[instructions.Pop()].Graveyard);
		}

		private void GetOwningCard()
		{
			// All Cards are stored in a list even if they're individual in case a further..
			// ..instruction requires to group a number of them together
			// instructions.Push(new List<Card>{card});
			cards.Add(Activated);
		}
		
		
		

		

	}
}
