using System.Collections.Generic;

public class EnemyModel : Internal.IModel<EnemyCommand, EnemyInfo>
{
    protected override void InitilizeController()
    {
        controller = new Internal.EnemyController(this);
    }

    protected override void ReceiveCommands(HashSet<EnemyCommand> commands)
    {
        foreach (EnemyCommand command in commands)
        {
            switch (command.Command)
            {
                case EnemyCommand.Type.Attack:
                    break;
            }
        }
    }

    protected override void Update()
    {
        float epsilon = 1e-5F;
        bool runningLeft = info.Rigidbody.velocity.x < -epsilon;
        bool runningRight = info.Rigidbody.velocity.x > epsilon;
        if (runningLeft)
            info.SpriteFlipper.TurnLeft();
        if (runningRight)
            info.SpriteFlipper.TurnRight();

        info.Sensor.RotationAroundYAxis = info.Movement.CurrentAngle;
        base.Update();
    }
}