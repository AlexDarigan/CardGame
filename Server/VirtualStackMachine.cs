using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server
{
	public class CustomStack
	{
		private List<int> list; // = new List<int>();
		public int Count => list.Count;

		public CustomStack(List<int> _listx) // => //list = _listx;
		{
			list = _listx;
		}

		public int Pop()
		{
			int popped = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return popped;
		}

		public void Push(int i)
		{
			list.Add(i);
		}
		
		public int this[int i] => list[i];
	}
	
	public class VirtualStackMachine
	{
		/*
		 * I was planning on grouping cards with a 0 to separate the values but suddenly realized if we store
		 * this as int it might be too big and cause overflow.
		 */
		

		public void Activate(Card card)
		{
			CustomStack instructions = new CustomStack(card.Skill.Instructions.ToList());
			IList<Player> players = new List<Player>{card.Controller, card.Controller.Opponent};
			List<Card> cards = new List<Card>();
			
			for(int index = 0; index < card.Skill.Instructions.Count(); index++)
			{
				Instructions instruction = (Instructions) instructions[index];
				switch (instruction)
				{
					
					case Instructions.Draw:
					{
						Player player = players[(int) instructions.Pop()];
						int count = (int) instructions.Pop();
						Draw(player, count);
					}
						break;
					case Instructions.Destroy:
					{
						Destroy(cards);
					}
						break;
					case Instructions.Count:
					{
						instructions.Push(cards.Count);
					}
						break;
					case Instructions.IfLessThan:
					{
						int a = instructions.Pop();
						int b = instructions.Pop();
						int jump = instructions.Pop();
						if (a < b)
						{
							// Do Nothing | Follow Until JUMP / GOTO / END
						}
						else
						{
							// Jump To Next Index - 1
							// - 1 is to account for the Index added by the for loop
							index += jump;
						}
					}
						break;
					case Instructions.IfGreaterThan:
						break;
					case Instructions.GetController:
						instructions.Push(0);
						break;
					case Instructions.GetOpponent:
						instructions.Push(1);
						break;
					case Instructions.GetDeck:
					{
						Player player = players[(int) instructions.Pop()];
						cards.AddRange(player.Deck);
					}
						break;
					case Instructions.GetGraveyard:
					{
						Player player = players[(int) instructions.Pop()];
						cards.AddRange(player.Graveyard);
					}
						break;
					case Instructions.GetHand:
					{
						Player player = players[(int) instructions.Pop()];
						cards.AddRange(player.Hand);
					}
						break;
					case Instructions.GetUnits:
					{
						Player player = players[(int) instructions.Pop()];
						cards.AddRange(player.Units);
					}
						break;
					case Instructions.GetSupport:
					{
						Player player = players[(int) instructions.Pop()];
						cards.AddRange(player.Supports);
					}
						break;
					case Instructions.GetOwningCard:
						// All Cards are stored in a list even if they're individual in case a further..
						// ..instruction requires to group a number of them together
					   // instructions.Push(new List<Card>{card});
						cards.Add(card);
						break;
					case Instructions.SetFaction:
					{
						Faction faction = (Faction) instructions.Pop();
						SetFaction(cards, faction);
					}
						break;
					case Instructions.SetPower:
					{
						int power= (int) instructions.Pop();
						SetPower(cards, power);
					}
						break;
					case Instructions.GoToEnd:
						index = instructions.Count; // We could just do an early return?
						break;
					case Instructions.DealDamage:
					{
						Player player = players[(int) instructions.Pop()];
						int damage = (int) instructions.Pop();
						player.Health -= damage;
					}
						break;
					case Instructions.Literal:
						index++;
						int elem = instructions[index];
						instructions.Push(elem);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void Draw(Player player, int count)
		{
			for (int i = 0; i < count; i++)
			{
				Card card = player.Deck[player.Deck.Count - 1];
				player.Deck.Remove(card);
				player.Hand.Add(card);
			}
		}

		private void Destroy(IEnumerable<Card> cards)
		{
			foreach (Card card in cards)
			{
				card.Zone.Remove(card);
				card.Owner.Graveyard.Add(card);
				card.Zone = card.Owner.Graveyard;
			}
		}

		private void SetTitle(IEnumerable<Card> cards, string title)
		{
			foreach (Card card in cards) { card.Title = title; }
		}
		
		private void SetFaction(IEnumerable<Card> cards, Faction faction)
		{
			foreach (Card card in cards) { card.Faction = faction; }
		}
		
		private void SetPower(IEnumerable<Card> cards, int power)
		{
			foreach (Card card in cards) { card.Power = power; }
		}

	}
}
