using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using CardGame;
using CardGame.Client;
using CardGame.Client.Commands;
using JetBrains.Annotations;

namespace CardGame.Client
{
    [UsedImplicitly]
    public class CommandQueue : Node
    {
        private delegate Command Invoker(params object[] args);

        private static Dictionary<CommandId, Invoker> Commands { get; } = new();
        private Queue<Command> Queue { get; } = new();

        static CommandQueue()
        {
            foreach (CommandId commandId in Enum.GetValues(typeof(CommandId)))
            {
                ConstructorInfo c = Type.GetType($"CardGame.Client.Commands.{Enum.GetName(commandId.GetType(), commandId)}")?.GetConstructors()[0];
                Commands[commandId] = args => (Command) c?.Invoke(args);
            }
        }

        public async void Execute(Room room) { while (Queue.Count > 0) { await Queue.Dequeue().Execute(room); } }
        public void Enqueue(CommandId commandId, object[] args) { Queue.Enqueue(Commands[commandId](args)); }
    }
}