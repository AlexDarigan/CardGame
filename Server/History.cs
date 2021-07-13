using System.Collections;
using System.Collections.Generic;
using CardGame.Server.Events;

namespace CardGame.Server
{
    public class History: IEnumerable<Event>
    {
        private readonly List<Event> _gameEvents = new();
        public void Add(Event gameEvent) { _gameEvents.Add(gameEvent); }
        
        public IEnumerator<Event> GetEnumerator() { return _gameEvents.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
    }
}