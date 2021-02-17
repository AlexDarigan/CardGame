﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server
{
    public class VirtualStackMachine
    {
        
        public readonly Stack<object> Arguments;
        
        public void Activate(Card card)
        {
            // Make sure these are unique copies
            IEnumerable<int> Instructions = card.Skill.Instructions;
            Stack<object> Arguments = card.Skill.Arguments;

            for(int index = 0; index < Instructions.Count(); index++)
            {
                Instructions instruction = (Instructions) Instructions.ElementAt(index);
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
                    case CardGame.Instructions.Count:
                    {
                        IList<Card> cards = (IList<Card>) Arguments.Pop();
                        Arguments.Push(cards.Count);
                    }
                        break;
                    case CardGame.Instructions.IfLessThan:
                    {
                        int a = (int) Arguments.Pop();
                        int b = (int) Arguments.Pop();
                        int jump = (int) Arguments.Pop();
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
                        Arguments.Push(card.Controller);
                        break;
                    case CardGame.Instructions.GetOpponent:
                        Arguments.Push(card.Controller.Opponent);
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
                        Arguments.Push(new List<Card>{card});
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
                    case CardGame.Instructions.DiscardArgument:
                        // Jumped In Control Flow so we discard Arguments we don't use
                        Arguments.Pop();
                        break;
                    case CardGame.Instructions.GoToEnd:
                        index = Instructions.Count(); // We could just do an early return?
                        break;
                    case CardGame.Instructions.DealDamage:
                    {
                        Player player = (Player) Arguments.Pop();
                        int damage = (int) Arguments.Pop();
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