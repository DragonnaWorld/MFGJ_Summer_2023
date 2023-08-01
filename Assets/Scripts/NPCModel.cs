using System.Collections.Generic;
using UnityEngine;

public class NPCModel : AIModel<NPCCommand, NPCInfo, NPCController>
{
    protected override void ReceiveCommands(HashSet<NPCCommand> commands)
    {
        foreach (var command in commands)
        {
            switch (command.CommandType)
            {
                case NPCCommand.Type.Move:
                    info.Movement.Move(command.Direction);
                    bool onlyLeft = command.Direction.HasFlag(OmniDirection.Left) && !command.Direction.HasFlag(OmniDirection.Right);
                    bool onlyRight = command.Direction.HasFlag(OmniDirection.Right) && !command.Direction.HasFlag(OmniDirection.Left);
                    if (onlyLeft)
                        info.SpriteFlipper.TurnLeft();
                    if (onlyRight)
                        info.SpriteFlipper.TurnRight();
                    break;
            }    
        }
    }    
}