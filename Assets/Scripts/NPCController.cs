using System;
using System.Collections.Generic;
using UnityEngine;

public struct NPCCommand
{
    public enum Type { Move };
    public Type CommandType;
    public OmniDirection Direction;
}

public class NPCController : AIController<NPCCommand, NPCInfo>
{
    readonly HashSet<NPCCommand> commands = new();
    float time = 0F;
    [SerializeField]
    [Range(1F, 5F)]
    float waitTime;

    public override HashSet<NPCCommand> Process()
    {
        commands.Clear();
        time += Time.deltaTime;
        if (time > waitTime)
        {
            time -= waitTime;
            AddDirection();
        }
        return commands;
    }

    void AddDirection()
    {
        var directions = (OmniDirection[])Enum.GetValues(typeof(OmniDirection));
        var directionCommand = new NPCCommand
        {
            CommandType = NPCCommand.Type.Move,
            Direction = directions[UnityEngine.Random.Range(0, directions.Length - 1)]
        };
        commands.Add(directionCommand);
        Debug.Log(directionCommand.Direction);
    }
}