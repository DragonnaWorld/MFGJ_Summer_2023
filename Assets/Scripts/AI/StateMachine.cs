using System.Collections.Generic;

namespace Internal
{
    public abstract class State<StateEnums, ModelInfo>
    {
        protected StateMachine<StateEnums, ModelInfo> stateMachine;
        protected ModelInfo info;

        public void Initialize(StateMachine<StateEnums, ModelInfo> stateMachine)
        {
            this.stateMachine = stateMachine;
            info = stateMachine.Info;
        }
        public virtual void Activate() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        public virtual void Deactivate() { }

        protected void ChangeState(StateEnums newState)
        {
            stateMachine.ChangeState(newState);
        }
    }

    public class StateMachine<StateEnums, ModelInfo> : IController<ModelInfo>
    {
        readonly Dictionary<StateEnums, State<StateEnums, ModelInfo>> states = new();
        
        StateEnums currentState;
        StateEnums pendingState;
        bool stateChanged = false;

        public StateMachine(ModelInfo info, StateEnums startState)
            : base(info)
        {
            ChangeState(startState);
        }

        public void Register<DerivedState>(StateEnums stateType)
            where DerivedState : State<StateEnums, ModelInfo>, new()
        {
            var state = new DerivedState();
            state.Initialize(this);
            states.Add(stateType, state);
        }

        public override void Update()
        {
            states[currentState].Update();
            CheckStateChange();
        }

        public override void FixedUpdate()
        {
            states[currentState].FixedUpdate();
            CheckStateChange();
        }

        public void ChangeState(StateEnums newState)
        {
            pendingState = newState;
            stateChanged = true;
        }

        void CheckStateChange()
        {
            if (stateChanged)
            {
                states[currentState].Deactivate();
                states[pendingState].Activate();
                currentState = pendingState;
                stateChanged = false;
            }
        }
    }
}