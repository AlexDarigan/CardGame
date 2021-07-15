namespace CardGame.Server.Events
{
    public abstract class Event
    {
        protected CommandId Command { get; set; }

        public abstract void QueueOnClients(Enqueue queue);
    }
}