using System.Collections.Generic;

public enum EnemyState
{
    Idle, Track,
}

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