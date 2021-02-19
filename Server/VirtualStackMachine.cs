using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace CardGame.Server
{
	public class VirtualStackMachine
	{
		private int index = 0;
		private Stack instructions;
		private IList<Player> players;
		List<Card> cards;
		private Card Activated;

		public void Activate(Card card)
		{
			Activated = card;
			int maxSize = card.Skill.Instructions.Count();
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
					{
						Player player = players[(int) instructions.Pop()];
						int count = (int) instructions.Pop();
						Draw(player, count);
					}
						break;
					case Instructions.Destroy:
						Destroy(cards);
						break;
					case Instructions.Count:
						instructions.Push(cards.Count);
						break;
					case Instructions.IsLessThan:
					{
						const int isFalse = 0;
						const int isTrue = 1;
						int a = instructions.Pop();
						int b = instructions.Pop();
						instructions.Push(a < b ? isTrue : isFalse);
					}
						break;
					case Instructions.If:
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
						break;
					case Instructions.GetController:
						instructions.Push(0);
						break;
					case Instructions.GetOpponent:
						instructions.Push(1);
						break;
					case Instructions.GetDeck:
						cards.AddRange(players[instructions.Pop()].Deck);
						break;
					case Instructions.GetGraveyard:
						cards.AddRange(players[instructions.Pop()].Graveyard);
						break;
					case Instructions.GetHand:
						cards.AddRange(players[instructions.Pop()].Hand);
						break;
					case Instructions.GetUnits:
						cards.AddRange(players[instructions.Pop()].Units);
						break;
					case Instructions.GetSupport:
						cards.AddRange(players[instructions.Pop()].Supports);
						break;
					case Instructions.GetOwningCard:
						// All Cards are stored in a list even if they're individual in case a further..
						// ..instruction requires to group a number of them together
					   // instructions.Push(new List<Card>{card});
						cards.Add(Activated);
						break;
					case Instructions.SetFaction:
						SetFaction(cards, (Faction) instructions.Pop());
						break;
					case Instructions.SetPower:
						SetPower(cards, instructions.Pop());
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
						instructions.Push(instructions[index]);
						break;
					case Instructions.IsGreaterThan:
					{
						const int isFalse = 0;
						const int isTrue = 1;
						int a = instructions.Pop();
						int b = instructions.Pop();
						instructions.Push(a > b ? isTrue : isFalse);
					}
						break;
					case Instructions.IsEqual:
					{
						const int isFalse = 0;
						const int isTrue = 1;
						int a = instructions.Pop();
						int b = instructions.Pop();
						instructions.Push(a == b ? isTrue : isFalse);
					}
						break;
					case Instructions.IsNotEqual:
					{
						const int isFalse = 0;
						const int isTrue = 1;
						int a = instructions.Pop();
						int b = instructions.Pop();
						instructions.Push(a != b ? isTrue : isFalse);
					}
						break;
					case Instructions.And:
					{
						const int isFalse = 0;
						const int isTrue = 1;
						int a = instructions.Pop();
						int b = instructions.Pop();
						instructions.Push(a == isTrue && b == isTrue ? isTrue : isFalse);
					}
						break;
					case Instructions.Or:
					{
						const int isFalse = 0;
						const int isTrue = 1;
						int a = instructions.Pop();
						int b = instructions.Pop();
						instructions.Push(a == isTrue || b == isTrue ? isTrue : isFalse);
					}
						break;
					default:
						throw new ArgumentOutOfRangeException();
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