using System.Collections.Generic;

namespace Enemy
{
    public class TrackState : Internal.State<EnemyState, EnemyCommand, EnemyInfo>
    {
        public override HashSet<EnemyCommand> Update()
        {
            return null;
        }
    }
}