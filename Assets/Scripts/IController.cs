using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract classes using model-controller pattern.
/// In each update, AIController will process the entity based on given information.
/// Then, commands are handed over to AIModel class, where the model **tries** to accomplish the commands given.
/// </summary>

public enum CommandStatus
{
    NotGiven, OnGoing, Finished, Failed
}

public abstract class AIController<Commands, ModelInfo> : MonoBehaviour where ModelInfo : MonoBehaviour
{
    protected ModelInfo info;

    protected virtual void Start()
    {
        info = GetComponent<ModelInfo>();
    }

    public abstract HashSet<Commands> Process();
}

public abstract class AIModel<Commands, ModelInfo, Controller> : MonoBehaviour 
    where Controller : AIController<Commands, ModelInfo> 
    where ModelInfo : MonoBehaviour  
{
    protected ModelInfo info;
    Controller controller;

    protected virtual void Start()
    {
        info = GetComponent<ModelInfo>();
        controller = GetComponent<Controller>(); 
    }

    protected virtual void Update()
    {
        var commands = controller.Process();
        ReceiveCommands(commands);
    }

    protected abstract void ReceiveCommands(HashSet<Commands> commands);
}