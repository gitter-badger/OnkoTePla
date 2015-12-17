using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.Lib.Utils.Workflows
{
    public class StateTransition<TState, TCommand>        
    {               
        public StateTransition(TState stateBefore, TCommand command, TState stateAfter)
        {
            StateBefore = stateBefore;
            Command = command;
            StateAfter = stateAfter;
        }

        public TState   StateBefore { get; }
        public TCommand Command     { get; }
        public TState   StateAfter  { get; }

        public override int GetHashCode()
        {
            return StateBefore.GetHashCode() ^ 
                   Command.GetHashCode() ^ 
                   StateAfter.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj, (st1, st2) => st1.StateBefore.Equals(st2) && 
                                                  st1.Command.Equals(st2) &&
                                                  st1.StateAfter.Equals(st2.StateAfter));
        }

        public override string ToString()
        {
            return $"[{StateBefore}] --- {Command} ---> [{StateAfter}]";
        }
    }
}