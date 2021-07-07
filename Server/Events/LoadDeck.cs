using System.Collections.Generic;

namespace CardGame.Server.Events
{
    public class LoadDeck : Event
    {
        private readonly Player Controller;
        private readonly Dictionary<int, SetCodes> Deck;

        public LoadDeck(Player controller, Dictionary<int, SetCodes> deck)
        {
            Command = CommandId.LoadDeck;
            Controller = controller;
            Deck = deck;
        }

        public override void QueueOnClients(Enqueue queue)
        {
            queue(Controller.Id, Command, isClient, Deck);
            queue(Controller.Opponent.Id, Command, !isClient, NullDeck());
        }

        private Dictionary<int, SetCodes> NullDeck()
        {
            Dictionary<int, SetCodes> nullDeck = new();
            for (int i = -1; i > -41; i--) nullDeck[i] = SetCodes.NullCard;

            return nullDeck;
        }
    }
}