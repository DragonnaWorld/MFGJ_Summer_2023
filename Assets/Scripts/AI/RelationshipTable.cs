using System.Collections.Generic;

namespace Internal
{
    public enum Relationship
    {
        Attack, Flee, Follow, Ignore
    }
    public class RelationshipTable
    {
        Dictionary<Relationship, HashSet<string>> table;
        
        public Relationship RelationshipTo(string gameObjectTag)
        {
            foreach (var pair in table)
                if (pair.Value.Contains(gameObjectTag))
                    return pair.Key;
            return Relationship.Ignore;
        }

        public static RelationshipTable Enemy = new()
        {
            table = new()
            {
                { Relationship.Attack, new HashSet<string> { "Player" } }
            }
        };
    }
}