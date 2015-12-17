using System;


namespace bytePassion.Lib.Utils.Workflows
{
    public interface IStateEngine<out TState, in TCommand>
    {
        event Action<TState> StateChanged;

        TState CurrentState { get; }

        bool IsCommandApplyable(TCommand command);
        TState MoveNext(TCommand command);
    }
}