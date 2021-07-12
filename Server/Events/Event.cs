namespace CardGame.Server.Events
{
    public abstract class Event
    {
        protected CommandId Command;

        public abstract void QueueOnClients(Enqueue queue);
    }
}