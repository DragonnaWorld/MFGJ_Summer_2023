using System.Collections.Generic;

public class EnemyModel : Internal.IModel<EnemyCommand, EnemyInfo>
{
    protected override void InitilizeController()
    {
        controller = new EnemyController(this);
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
        info.Sensor.RotationAroundYAxis = 90 - info.Movement.CurrentAngleToOx;
        base.Update();
    }
}