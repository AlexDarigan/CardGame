using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server
{
	public class VirtualStackMachine
	{
		/*
		 * I was planning on grouping cards with a 0 to separate the values but suddenly realized if we store
		 * this as int it might be too big and cause overflow.
		 */
		public void Activate(Card card)
		{
			// Make sure these are unique copies
			Stack<int> instructions = new Stack<int>(new Stack<int>(card.Skill.Instructions));
			//instructions.Reverse();
			Stack<object> arguments = card.Skill.Arguments;
			IList<Player> players = new List<Player>{card.Controller, card.Controller.Opponent};
			List<Card> cards = new List<Card>();
			
			for(int index = 0; index < instructions.Count(); index++)
			{
				Instructions instruction = (Instructions) instructions.ElementAt(index);
				switch (instruction)
				{
					case Instructions.Draw:
					{
						Player player = players[(int) arguments.Pop()];
						int count = (int) arguments.Pop();
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
						arguments.Push(cards.Count);
					}
						break;
					case Instructions.IfLessThan:
					{
						int a = (int) arguments.Pop();
						int b = (int) arguments.Pop();
						int jump = (int) arguments.Pop();
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
						arguments.Push(0);
						break;
					case Instructions.GetOpponent:
						arguments.Push(1);
						break;
					case Instructions.GetDeck:
					{
						Player player = players[(int) arguments.Pop()];
						cards.AddRange(player.Deck);
					}
						break;
					case Instructions.GetGraveyard:
					{
						Player player = players[(int) arguments.Pop()];
						cards.AddRange(player.Graveyard);
					}
						break;
					case Instructions.GetHand:
					{
						Player player = players[(int) arguments.Pop()];
						cards.AddRange(player.Hand);
					}
						break;
					case Instructions.GetUnits:
					{
						Player player = players[(int) arguments.Pop()];
						cards.AddRange(player.Units);
					}
						break;
					case Instructions.GetSupport:
					{
						Player player = players[(int) arguments.Pop()];
						cards.AddRange(player.Supports);
					}
						break;
					case Instructions.GetOwningCard:
						// All Cards are stored in a list even if they're individual in case a further..
						// ..instruction requires to group a number of them together
					   // arguments.Push(new List<Card>{card});
						cards.Add(card);
						break;
					case Instructions.SetTitle:
					{
						string title = (string) arguments.Pop();
						SetTitle(cards, title);
					}
						break;
					case Instructions.SetFaction:
					{
						Enum.TryParse((string) arguments.Pop(), out Faction faction);
						SetFaction(cards, faction);
					}
						break;
					case Instructions.SetPower:
					{
						int power= (int) arguments.Pop();
						SetPower(cards, power);
					}
						break;
					case Instructions.DiscardArgument:
						// Jumped In Control Flow so we discard Arguments we don't use
						arguments.Pop();
						break;
					case Instructions.GoToEnd:
						index = instructions.Count(); // We could just do an early return?
						break;
					case Instructions.DealDamage:
					{
						Player player = players[(int) arguments.Pop()];
						int damage = (int) arguments.Pop();
						player.Health -= damage;
					}
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
