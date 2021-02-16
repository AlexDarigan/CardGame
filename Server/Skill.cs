using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;

namespace CardGame.Server
{
    public class Skill
    {
        public readonly Card Owner;
        public readonly IEnumerable<Triggers> Triggers;
        public readonly IEnumerable<Instructions> Instructions;
        public readonly string Description;
        public Stack<object> Arguments;

        public Skill(Card owner, IEnumerable<Triggers> triggers, IEnumerable<Instructions> instructions, string desc)
        {
            Owner = owner;
            Triggers = triggers;
            Instructions = instructions;
            Description = desc;
        }

        public void Activate()
        {
            Arguments = new Stack<object>();
             foreach (Instructions instruction in Instructions)
             {
                 switch (instruction)
                 {
                     case CardGame.Instructions.Zero:
                         Arguments.Push(0);
                         break;
                     case CardGame.Instructions.One:
                         Arguments.Push(1);
                         break;
                     case CardGame.Instructions.Two:
                         Arguments.Push(2);
                         break;
                     case CardGame.Instructions.Three:
                         Arguments.Push(3);
                         break;
                     case CardGame.Instructions.Four:
                         Arguments.Push(4);
                         break;
                     case CardGame.Instructions.Five:
                         Arguments.Push(5);
                         break;
                     case CardGame.Instructions.Six:
                         Arguments.Push(6);
                         break;
                     case CardGame.Instructions.Seven:
                         Arguments.Push(7);
                         break;
                     case CardGame.Instructions.Eight:
                         Arguments.Push(8);
                         break;
                     case CardGame.Instructions.Nine:
                         Arguments.Push(9);
                         break;
                     case CardGame.Instructions.Ten:
                         Arguments.Push(10);
                         break;
                     case CardGame.Instructions.Hundred:
                         Arguments.Push(100);
                         break;
                     case CardGame.Instructions.Thousand:
                         Arguments.Push(1000);
                         break;
                     case CardGame.Instructions.Draw:
                         int count = (int) Arguments.Pop();
                         Player player = (Player) Arguments.Pop();
                         Draw(player, count);
                         break;
                     case CardGame.Instructions.GetController:
                         Arguments.Push(Owner.Controller);
                         break;
                     default:
                         throw new ArgumentOutOfRangeException();
                 }
            }
        }

        public void Draw(Player player, int count)
        {
            for (int i = 0; i < 2; i++)
            {
                Card card = player.Deck[player.Deck.Count - 1];
                player.Deck.Remove(card);
                player.Hand.Add(card);
            }
        }
    }
}