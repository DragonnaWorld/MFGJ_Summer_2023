using System.Collections.Generic;

public class EnemyController 
    : Internal.StateMachine<EnemyState, EnemyCommand, EnemyInfo>, Internal.IController<EnemyCommand>
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