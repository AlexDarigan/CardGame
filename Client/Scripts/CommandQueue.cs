using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Commands
{
    public class CommandQueue: Tween
    {
        private Queue<Command> Queue { get; } = new();
        private Player Player { get; }
        private Rival Rival { get; }
        private Cards Cards { get; }

        public CommandQueue(Player player, Rival rival, Cards cards)
        {
            Player = player;
            Rival = rival;
            Cards = cards;
        }

        public void Enqueue(CommandId commandId, params object[] args)
        {
            // Command command = (Command) typeof(CommandQueue).GetMethod(commandId.ToString(), 
            //     BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(this, args);
            // if (command is null)
            // {
            //     throw new NullReferenceException("No Command Found");
            // }
            // Queue.Enqueue(command);
            Queue.Enqueue((Command) Call(commandId.ToString(), args));
            
        }
        
        
        public async Task Execute()
        {
            while (Queue.Count > 0) await Queue.Dequeue().Execute(this);
        }
        
       public Participant GetPlayer(bool isPlayer) { return isPlayer ? Player : Rival; }
       public Card GetCard(int id, SetCodes setCodes = SetCodes.NullCard) { return Cards.GetCard(id, setCodes);}
        
        private Command LoadDeck(bool who, Dictionary<int, SetCodes> deck) { return new LoadDeck(GetPlayer(who), deck, Cards.GetCard);} 
        private Command Draw(bool who, int id) { return new Draw(who, id); }
        private Command Deploy(bool who, int id, SetCodes setCodes = SetCodes.NullCard) { return new Deploy(who, id, setCodes);}
        private Command SetFaceDown(bool who, int id) { return new SetFaceDown(who, id);}
        private Command SetHealth(bool who, int health) { return new SetHealth(who, health);}
        private Command SentToGraveyard(int id) { return new SentToGraveyard(id);}
        private Command Battle(int attacker, int defender) { return new Battle(attacker, defender);}
    }
}