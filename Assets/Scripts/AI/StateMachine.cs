using System.Collections.Generic;

namespace Internal
{
    public abstract class State<StateEnums, Commands, ModelInfo>
    {
        protected StateMachine<StateEnums, Commands, ModelInfo> stateMachine;
        protected ModelInfo info;

        public void Initialize(StateMachine<StateEnums, Commands, ModelInfo> stateMachine) 
        {
            this.stateMachine = stateMachine;
            info = stateMachine.GetModelInfo();
        }
        public virtual void Activate() { }
        public abstract HashSet<Commands> Update();
        public virtual void Deactivate() { }

        protected void ChangeState(StateEnums newState)
        {
            stateMachine.ChangeState(newState);
        }
    }

    public class StateMachine<StateEnums, Commands, ModelInfo>
    {
        readonly ModelInfo info;
        readonly Dictionary<StateEnums, State<StateEnums, Commands, ModelInfo>> states = new();
        readonly HashSet<Commands> emptyCommandSet = new();
        
        StateEnums currentState;
        StateEnums pendingState;
        bool stateChanged = false;

        public StateMachine(ModelInfo info, StateEnums startState)
        {
            this.info = info;
            ChangeState(startState);
        }

        public void Register<DerivedState>(StateEnums stateType)
            where DerivedState : State<StateEnums, Commands, ModelInfo>, new()
        {
            State<StateEnums, Commands, ModelInfo> state = new DerivedState();
            state.Initialize(this);
            states.Add(stateType, state);
        }

        public HashSet<Commands> Update()
        {
            var res = states[currentState].Update();

            if (stateChanged)
            {
                states[currentState].Deactivate();
                states[pendingState].Activate();
                currentState = pendingState;
                stateChanged = false;
            }

            return res == null ? emptyCommandSet : res;
        }

        public void ChangeState(StateEnums newState)
        {
            pendingState = newState;
            stateChanged = true;
        }

        public ModelInfo GetModelInfo()
        {
            return info;
        }
    }
}