using System.Collections.Generic;

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
                    break;
            }    
        }
    }    
}