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
				Execute(instruction);
			}

		}
		
		private void Execute(Instructions instruction)
		{
			switch (instruction)
				{
					case Instructions.Draw:
						Draw();
						break;
					case Instructions.Destroy:
						Destroy();
						break;
					case Instructions.Count:
						Count();
						break;
					case Instructions.IsLessThan:
						IsLessThan();
						break;
					case Instructions.If:
						If();
						break;
					case Instructions.GetController:
						GetController();
						break;
					case Instructions.GetOpponent:
						GetOpponent();
						break;
					case Instructions.GetDeck:
						GetDeck();
						break;
					case Instructions.GetGraveyard:
						GetGraveyard();
						break;
					case Instructions.GetHand:
						GetHand();
						break;
					case Instructions.GetUnits:
						GetUnits();
						break;
					case Instructions.GetSupport:
						GetSupports();
						break;
					case Instructions.GetOwningCard:
						GetOwningCard();
						break;
					case Instructions.SetFaction:
						SetFaction();
						break;
					case Instructions.SetPower:
						SetPower();
						break;
					case Instructions.GoToEnd:
						GoToEnd();
						break;
					case Instructions.DealDamage:
						DealDamage();
						break;
					case Instructions.Literal:
						Literal();
						break;
					case Instructions.IsGreaterThan:
						IsGreaterThan();
						break;
					case Instructions.IsEqual:
						IsEqual();
						break;
					case Instructions.IsNotEqual:
						IsNotEqual();
						break;
					case Instructions.And:
						And();
						break;
					case Instructions.Or:
						Or();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
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
