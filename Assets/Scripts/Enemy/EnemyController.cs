using System.Collections.Generic;

namespace Internal
{
    public class EnemyController
        : StateMachine<EnemyState, EnemyInfo>
    {
        public EnemyController(EnemyInfo info)
            : base(info, EnemyState.Idle)
        {
            Register<Enemy.IdleState>(EnemyState.Idle);
            Register<Enemy.TrackState>(EnemyState.Track);
        }
    }
}