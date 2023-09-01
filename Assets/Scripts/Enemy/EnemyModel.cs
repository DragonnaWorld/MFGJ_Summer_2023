using Internal;

public class EnemyModel : IModel<EnemyInfo>
{
    protected override IController<EnemyInfo> CreateController(EnemyInfo info)
    {
        return new EnemyController(info);
    }

    protected override void UpdateModel()
    {
        float tolerance = 10F;
        bool runningLeft = info.Movement.CurrentAngle > 180F + tolerance;
        bool runningRight = info.Movement.CurrentAngle < 180F - tolerance;
        if (runningLeft)
            info.SpriteFlipper.TurnLeft();
        else if (runningRight)
            info.SpriteFlipper.TurnRight();

        info.Sensor.RotationAroundYAxis = info.Movement.CurrentAngle;
    }
}