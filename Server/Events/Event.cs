using System;

namespace CardGame.Server
{
    public abstract class Event
    {
        protected CommandId Command;

        public abstract void QueueOnClients(Enqueue queue);
    }
}