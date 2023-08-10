using UnityEngine;

namespace Internal
{
    public abstract class IController<ModelInfo>
    {
        public ModelInfo Info { get; private set; }
        protected IController(ModelInfo info)
        {
           Info = info;
        }

        public abstract void Update();
    }

    public abstract class IModel<ModelInfo> : MonoBehaviour
        where ModelInfo : MonoBehaviour
    {
        protected ModelInfo info;
        IController<ModelInfo> controller;

        private void Start()
        {
            info = GetComponent<ModelInfo>();
            controller = CreateController(info);
            Initialize();
        }

        private void Update()
        {
            controller.Update();
            UpdateModel();
        }

        protected abstract IController<ModelInfo> CreateController(ModelInfo info);
        protected virtual void Initialize() { }
        protected abstract void UpdateModel();
    }
}