using System;


namespace bytePassion.Lib.Communication.State
{

    public interface IGlobalStateReadOnly<out T>
    {
        event Action<T> StateChanged;

        T Value { get; }
    }
}