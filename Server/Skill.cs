using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;
using Godot;

namespace CardGame.Server
{
    public class Skill
    {
        public readonly Card Owner;
        public readonly IEnumerable<Triggers> Triggers;
        public readonly IEnumerable<Instructions> Instructions;
        public readonly Stack<object> Arguments;
        public readonly string Description;

        public Skill(Card owner, IEnumerable<Triggers> triggers, IEnumerable<Instructions> instructions, 
            Stack<object> arguments, string description)
        {
            Owner = owner;
            Triggers = triggers;
            Instructions = instructions;
            Arguments = arguments;
            Description = description;
        }

        public void Activate()
        {
            foreach (Instructions instruction in Instructions)
            {
                switch (instruction)
                {
                    case CardGame.Instructions.Draw:
                    {
                        Player player = (Player) Arguments.Pop();
                        int count = (int) Arguments.Pop();
                        Draw(player, count);
                    }
                        break;
                    case CardGame.Instructions.Destroy:
                    {
                        IList<Card> cards = (IList<Card>) Arguments.Pop();
                        Destroy(cards);
                    }
                        break;
                    case CardGame.Instructions.GetController:
                        Arguments.Push(Owner.Controller);
                        break;
                    case CardGame.Instructions.GetOpponent:
                        Arguments.Push(Owner.Controller.Opponent);
                        break;
                    case CardGame.Instructions.GetDeck:
                    {
                        Player player = (Player) Arguments.Pop();
                        Arguments.Push(new List<Card>(player.Deck));
                    }
                        break;
                    case CardGame.Instructions.GetGraveyard:
                    {
                        Player player = (Player) Arguments.Pop();
                        Arguments.Push(new List<Card>(player.Graveyard));
                    }
                        break;
                    case CardGame.Instructions.GetHand:
                    {
                        Player player = (Player) Arguments.Pop();
                        Arguments.Push(new List<Card>(player.Hand));
                    }
                        break;
                    case CardGame.Instructions.GetUnits:
                    {
                        Player player = (Player) Arguments.Pop();
                        Arguments.Push(new List<Card>(player.Units));
                    }
                        break;
                    case CardGame.Instructions.GetSupport:
                    {
                        Player player = (Player) Arguments.Pop();
                        Arguments.Push(new List<Card>(player.Supports));
                    }
                        break;
                    case CardGame.Instructions.GetOwningCard:
                        // All Cards are stored in a list even if they're individual in case a further..
                        // ..instruction requires to group a number of them together
                        Arguments.Push(new List<Card>{Owner});
                        break;
                    case CardGame.Instructions.SetTitle:
                    {
                        IList<Card> cards = (IList<Card>) Arguments.Pop();
                        string title = (string) Arguments.Pop();
                        SetTitle(cards, title);
                    }
                        break;
                    case CardGame.Instructions.SetFaction:
                    {
                        IList<Card> cards = (IList<Card>) Arguments.Pop();
                        Enum.TryParse((string) Arguments.Pop(), out Faction faction);
                        SetFaction(cards, faction);
                    }
                        break;
                    case CardGame.Instructions.SetPower:
                    {
                        IList<Card> cards = (IList<Card>) Arguments.Pop();
                        int power= (int) Arguments.Pop();
                        SetPower(cards, power);
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