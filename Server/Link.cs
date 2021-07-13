using System.Collections.Generic;
using CardGame.Server.Events;

namespace CardGame.Server
{
    public class Link
    {
        // Not entirely accurate since this is also true at activation time
        public bool IsResolving => SkillStates.Count > 0;
        private List<SkillState> SkillStates { get; } = new();

        public void Add(SkillState skillState)
        {
            SkillStates.Add(skillState);
        }
        
        // If we're going to upgrade the link we're going to have to fix our tests to use pass play options
        public IEnumerable<Event> Resolve()
        {
            // Quick Note but maybe we could make this a generator method? That means we loop outside
            // We're using a while but do we have something better to use?
                // Shouldn't we be able to use an actual csharp stack but I remember that being
                // ..a problem from the last time? Or maybe that was only with operations

                SkillState current = SkillStates[SkillStates.Count - 1];
                IEnumerable<Event> gameEvents = current.Execute();
                SkillStates.Remove(current);
                
                // Feels like these could be fitted directly onto current?
                current.Controller.Supports.Remove(current.OwningCard);
                current.Owner.Graveyard.Remove(current.OwningCard);
                return gameEvents;
        }
    }
}