namespace CardGame.Server.Events
{
    public abstract class Event
    {
        protected const bool isClient = true;
        protected CommandId Command;

        public abstract void QueueOnClients(Enqueue queue);
    }
}