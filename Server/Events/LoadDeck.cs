using System.Collections.Generic;
using System.Linq;

namespace CardGame.Server.Events
{
    public class LoadDeck : Event
    {
        private Player Controller { get; }
        private Dictionary<int, SetCodes> Deck { get; }

        public LoadDeck(Player controller, Dictionary<int, SetCodes> deck)
        {
            Controller = controller;
            Deck = deck;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, CommandId.PlayerLoadDeck, Deck.AsEnumerable());
            queue(Controller.Opponent.Id, CommandId.RivalLoadDeck);
        }
    }
}