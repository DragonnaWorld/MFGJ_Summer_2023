using Internal;
using Player;

public enum PlayerState
{
    IdleAndMove,
    Attack,
}

public class PlayerController : StateMachine<PlayerState, PlayerInfo>
{
    public PlayerController(PlayerInfo info)
        : base(info, PlayerState.IdleAndMove)
    {
        Register<IdleAndMovingState>(PlayerState.IdleAndMove);
        Register<AttackState>(PlayerState.Attack);
    }
}