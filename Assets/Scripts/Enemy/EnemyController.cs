using System.Collections.Generic;

namespace Internal
{
    public class EnemyController
        : StateMachine<EnemyState, EnemyCommand, EnemyInfo>, IController<EnemyCommand>
    {
        public EnemyController(EnemyModel model)
            : base(model.Info, EnemyState.Idle)
        {
            Register<Enemy.IdleState>(EnemyState.Idle);
            Register<Enemy.TrackState>(EnemyState.Track);
        }

        public HashSet<EnemyCommand> Process()
        {
            return Update();
        }
    }
}