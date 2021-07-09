using System;
using System.Collections.Generic;
using Godot;

namespace CardGame.Client.Commands
{
    public class CommandQueue: Tween
    {
        private Queue<Command> Queue { get; } = new();
        private Player Player { get; }
        private Rival Rival { get; }
        private Cards Cards { get; }
        public int Count => Queue.Count;
        
        public CommandQueue(Player player, Rival rival, Cards cards)
        {
            Player = player;
            Rival = rival;
            Cards = cards;
        }

        public void Enqueue(CommandId commandId, params object[] args)
        {
            Queue.Enqueue((Command) Call(commandId.ToString(), args));
        }

        public Command Dequeue()
        {
            return Queue.Dequeue(); //.Execute(this);
        }
        
        private Participant GetPlayer(bool isPlayer) { return isPlayer ? Player : Rival; }
        private Card GetCard(int id, SetCodes setCodes = SetCodes.NullCard) { return Cards.GetCard(id, setCodes);}
        
        private Command LoadDeck(bool who, Dictionary<int, SetCodes> deck) { return new LoadDeck(GetPlayer(who), deck, Cards.GetCard);} 
        private Command Draw(bool who, int id) { return new Draw(GetPlayer(who), GetCard(id)); }
        private Command Deploy(bool who, int id, SetCodes setCodes) { return new Deploy(GetPlayer(who), GetCard(id, setCodes));}
        private Command SetFaceDown(bool who, int id) { return new SetFaceDown(GetPlayer(who), GetCard(id));}
        private Command SetHealth(bool who, int health) { return new SetHealth(GetPlayer(who), health);}
        private Command SentToGraveyard(int id) { return new SentToGraveyard(GetCard(id));}
        private Command Battle(int attacker, int defender) { return new Battle(GetCard(attacker), GetCard(defender));}
    }
}