using System;
using System.Threading.Tasks;

namespace CardGame.Client.Commands
{
    public abstract class Command
    {
        
        // Move Command
        // Set Player/Card Property Command
        // Battle Command (Movement + SetHealth?)
        // Activation Command (Movement?)
        // Resolve Command (Special)?
        
        // Store common operations down here so we can be more declarative in subclasses
        public async Task Execute(Room room)
        {
            room.Effects.RemoveAll();
            Setup(room);
            room.Effects.Start();
            await room.Effects.Executed();
        }

        protected abstract void Setup(Room room);

        private static int count = 0;
        protected void Move(Room room, Card card, Zone destination)
        {
            Zone origin = card.CurrentZone;
            Console.WriteLine($"{count}: {card.CurrentZone.Name}");
            count++;
            origin.Remove(card);
            destination.Add(card);
        }
    }
}