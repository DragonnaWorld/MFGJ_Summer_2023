public enum EnemyState
{
    Idle, Track, Attack
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
            //Register<Enemy.SightTrackState>(EnemyState.Track);
            Register<Enemy.PathfindingTrackState>(EnemyState.Track);
            Register<Enemy.AttackState>(EnemyState.Attack);
        }
    }
}