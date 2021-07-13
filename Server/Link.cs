using System.Collections.Generic;

namespace CardGame.Server
{
    public class Link
    {
        private List<SkillState> _link = new List<SkillState>();

        public void Add(SkillState skillState)
        {
            _link.Add(skillState);
        }
        
        // If we're going to upgrade the link we're going to have to fix our tests to use pass play options
        public void Resolve()
        {
            // Quick Note but maybe we could make this a generator method? That means we loop outside
            while (_link.Count > 0)
            {
                // We're using a while but do we have something better to use?
                // Shouldn't we be able to use an actual csharp stack but I remember that being
                // ..a problem from the last time? Or maybe that was only with operations

                SkillState current = _link[_link.Count - 1];
                current.Execute();
                if(!current.IsDone()) continue;
                _link.Remove(current);
                
                // Feels like these could be fitted directly onto current?
                current.Controller.Supports.Remove(current.OwningCard);
                current.Owner.Graveyard.Remove(current.OwningCard);
            }
        }
    }
}