using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Commands
{
    public class CommandQueue: Tween
    {
        private delegate Command Invoker(params object[] args);
        private Dictionary<CommandId, Invoker> Commands { get; } = new();
        private Queue<Command> Queue { get; } = new();
        private Player Player { get; }
        private Rival Rival { get; }
        private Cards Cards { get; }

        public CommandQueue(Player player, Rival rival, Cards cards)
        {
            Player = player;
            Rival = rival;
            Cards = cards;
            
            foreach (CommandId commandId in Enum.GetValues(typeof(CommandId)))
            {
                ConstructorInfo c = Type.GetType($"CardGame.Client.Commands.{commandId.ToString()}")?.GetConstructors()[0];
                Commands[commandId] = (args) => (Command) c?.Invoke(args);
            }
        }
        
        public async Task Execute() { while (Queue.Count > 0) { await Queue.Dequeue().Execute(this); } }
        public void Enqueue(CommandId commandId, object[] args) { Queue.Enqueue(Commands[commandId](args)); }
        public Participant GetPlayer(bool isPlayer) { return isPlayer ? Player : Rival; }
        public Card GetCard(int id, SetCodes setCodes = SetCodes.NullCard) { return Cards.GetCard(id, setCodes);}
        
    }
}