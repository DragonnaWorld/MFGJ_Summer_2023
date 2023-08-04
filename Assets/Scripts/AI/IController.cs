using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract classes using model-controller pattern.
/// In each update, AIController will process the entity based on given information.
/// Then, commands are handed over to AIModel class, where the model **tries** to accomplish the commands given.
/// </summary>

namespace Internal
{
    public interface IController<Commands>
    {
        public abstract HashSet<Commands> Process();
    }

    public abstract class IModel<Commands, ModelInfo> : MonoBehaviour
        where ModelInfo : MonoBehaviour
    {
        protected ModelInfo info;
        protected IController<Commands> controller;

        protected virtual void Start()
        {
            info = GetComponent<ModelInfo>();
            InitilizeController();
        }

        protected virtual void Update()
        {
            var commands = controller.Process();
            ReceiveCommands(commands);
        }

        public ModelInfo Info { get { return info; } }

        protected abstract void ReceiveCommands(HashSet<Commands> commands);
        protected abstract void InitilizeController();
    }
}