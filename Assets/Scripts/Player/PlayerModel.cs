using Internal;

public class PlayerModel : IModel<PlayerInfo>
{
    protected override IController<PlayerInfo> CreateController(PlayerInfo info)
    {
        return new PlayerController(info);
    }

    protected override void UpdateModel()
    {
        float xVelocity = info.Rgbody.velocity.x;
        float tolerance = 1e-5F;
        if (xVelocity > tolerance)
        {
            info.SpriteFlipper.TurnRight();
        }
        if (xVelocity < -tolerance)
        {
            info.SpriteFlipper.TurnLeft();
        }
    }
}