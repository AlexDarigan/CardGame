namespace CardGame.Server.Events
{
    public abstract class Event
    {
        public abstract void QueueOnClients(Enqueue queue);
    }
}