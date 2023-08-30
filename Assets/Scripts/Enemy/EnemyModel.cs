using Internal;

public class EnemyModel : IModel<EnemyInfo>
{
    protected override IController<EnemyInfo> CreateController(EnemyInfo info)
    {
        return new EnemyController(info);
    }

    protected override void UpdateModel()
    {
        bool runningLeft = info.Movement.CurrentAngle > 180F;
        bool runningRight = info.Movement.CurrentAngle < 180F;
        if (runningLeft)
            info.SpriteFlipper.TurnLeft();
        else if (runningRight)
            info.SpriteFlipper.TurnRight();

        info.Sensor.RotationAroundYAxis = info.Movement.CurrentAngle;
    }
}