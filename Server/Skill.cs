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
                         break;
                     case CardGame.Instructions.One:
                         break;
                     case CardGame.Instructions.Two:
                         Arguments.Push(2);
                         break;
                     case CardGame.Instructions.Three:
                         break;
                     case CardGame.Instructions.Four:
                         break;
                     case CardGame.Instructions.Five:
                         break;
                     case CardGame.Instructions.Six:
                         break;
                     case CardGame.Instructions.Seven:
                         break;
                     case CardGame.Instructions.Eight:
                         break;
                     case CardGame.Instructions.Nine:
                         break;
                     case CardGame.Instructions.Ten:
                         break;
                     case CardGame.Instructions.Hundred:
                         break;
                     case CardGame.Instructions.Thousand:
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