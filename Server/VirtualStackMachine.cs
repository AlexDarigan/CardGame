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
        private readonly CardRegister _cardRegister;

        public VirtualStackMachine(CardRegister cardRegister)
        {
            _cardRegister = cardRegister;
        }
        public void Activate(Card card)
        {
            // Make sure these are unique copies
            IEnumerable<int> Instructions = card.Skill.Instructions;
            Stack<object> arguments = card.Skill.Arguments;
            IList<Player> players = new List<Player>{card.Controller, card.Controller.Opponent};
            IList<Card> Cards = new List<Card>();
            
            for(int index = 0; index < Instructions.Count(); index++)
            {
                Instructions instruction = (Instructions) Instructions.ElementAt(index);
                switch (instruction)
                {
                    case CardGame.Instructions.Draw:
                    {
                        Player player = players[(int) arguments.Pop()];
                        int count = (int) arguments.Pop();
                        Draw(player, count);
                    }
                        break;
                    case CardGame.Instructions.Destroy:
                    {
                        IList<Card> cards = (IList<Card>) arguments.Pop();
                        Destroy(cards);
                    }
                        break;
                    case CardGame.Instructions.Count:
                    {
                        IList<Card> cards = (IList<Card>) arguments.Pop();
                        arguments.Push(cards.Count);
                    }
                        break;
                    case CardGame.Instructions.IfLessThan:
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
                    case CardGame.Instructions.IfGreaterThan:
                        break;
                    case CardGame.Instructions.GetController:
                        arguments.Push(0);
                        break;
                    case CardGame.Instructions.GetOpponent:
                        arguments.Push(1);
                        break;
                    case CardGame.Instructions.GetDeck:
                    {
                        Player player = players[(int) arguments.Pop()];
                        arguments.Push(new List<Card>(player.Deck));
                    }
                        break;
                    case CardGame.Instructions.GetGraveyard:
                    {
                        Player player = players[(int) arguments.Pop()];
                        arguments.Push(new List<Card>(player.Graveyard));
                    }
                        break;
                    case CardGame.Instructions.GetHand:
                    {
                        Player player = players[(int) arguments.Pop()];
                        arguments.Push(new List<Card>(player.Hand));
                    }
                        break;
                    case CardGame.Instructions.GetUnits:
                    {
                        Player player = players[(int) arguments.Pop()];
                        arguments.Push(new List<Card>(player.Units));
                    }
                        break;
                    case CardGame.Instructions.GetSupport:
                    {
                        Player player = players[(int) arguments.Pop()];
                        arguments.Push(new List<Card>(player.Supports));
                    }
                        break;
                    case CardGame.Instructions.GetOwningCard:
                        // All Cards are stored in a list even if they're individual in case a further..
                        // ..instruction requires to group a number of them together
                        arguments.Push(new List<Card>{card});
                        break;
                    case CardGame.Instructions.SetTitle:
                    {
                        IList<Card> cards = (IList<Card>) arguments.Pop();
                        string title = (string) arguments.Pop();
                        SetTitle(cards, title);
                    }
                        break;
                    case CardGame.Instructions.SetFaction:
                    {
                        IList<Card> cards = (IList<Card>) arguments.Pop();
                        Enum.TryParse((string) arguments.Pop(), out Faction faction);
                        SetFaction(cards, faction);
                    }
                        break;
                    case CardGame.Instructions.SetPower:
                    {
                        IList<Card> cards = (IList<Card>) arguments.Pop();
                        int power= (int) arguments.Pop();
                        SetPower(cards, power);
                    }
                        break;
                    case CardGame.Instructions.DiscardArgument:
                        // Jumped In Control Flow so we discard Arguments we don't use
                        arguments.Pop();
                        break;
                    case CardGame.Instructions.GoToEnd:
                        index = Instructions.Count(); // We could just do an early return?
                        break;
                    case CardGame.Instructions.DealDamage:
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