using System;
using System.Collections.Generic;
using System.Linq;


namespace bytePassion.Lib.Utils.Workflows
{

    public class StateEngine<TState, TCommand> : IStateEngine<TState, TCommand>
    {
        public event Action<TState> StateChanged;

        private readonly IReadOnlyList<StateTransition<TState, TCommand>> transitions;

        private TState currentState;

        public TState CurrentState
        {
            get { return currentState; }
            private set
            {
                if (!currentState.Equals(value))
                {
                    currentState = value;
                    StateChanged?.Invoke(CurrentState);
                }
            }
        }

        public StateEngine(IReadOnlyList<StateTransition<TState, TCommand>> transitions, 
                           TState initialState)
        {
            this.transitions = transitions;
            CurrentState = initialState;            
        } 

        public bool IsCommandApplyable(TCommand command)
        {
            return transitions.Any(transition => transition.Command.Equals(command) &&
                                                 transition.StateBefore.Equals(CurrentState));
        }
        
        public TState MoveNext(TCommand command)
        {
            var currentTransition = transitions.FirstOrDefault(transition => transition.Command.Equals(command) &&
                                                                             transition.StateBefore.Equals(CurrentState));
            if (currentTransition == null)
                throw new IllegalStateTransitionException($"there is no transition from {CurrentState} with the Command {command}");
            
            CurrentState = currentTransition.StateAfter;
            return CurrentState;
        }
    }
}
