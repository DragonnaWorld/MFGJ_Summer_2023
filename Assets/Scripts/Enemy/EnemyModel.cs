using Internal;

public class EnemyModel : IModel<EnemyInfo>
{
    protected override IController<EnemyInfo> CreateController(EnemyInfo info)
    {
        return new EnemyController(info);
    }

    protected override void UpdateModel()
    {
        bool runningLeft = info.Rigidbody.velocity.x < -1;
        bool runningRight = info.Rigidbody.velocity.x > 1;
        if (runningLeft)
            info.SpriteFlipper.TurnLeft();
        else if (runningRight)
            info.SpriteFlipper.TurnRight();

        info.Sensor.RotationAroundYAxis = info.Movement.CurrentAngle;
    }
}