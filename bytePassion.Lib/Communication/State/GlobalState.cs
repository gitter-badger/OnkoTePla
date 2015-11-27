using System;


namespace bytePassion.Lib.Communication.State
{

    public class GlobalState<T> : IGlobalState<T>,
                                  IGlobalStateReadOnly<T>,
                                  IGlobalStateWriteOnly<T>
    {
        public event Action<T> StateChanged;

        private T stateValue;

        public GlobalState(T initialValue)
        {
            stateValue = initialValue;
        }

        public GlobalState() : this(default(T))
        {
        }

        public T Value
        {
            get { return stateValue; }
            set
            {
                if (Equals(value, stateValue)) return;

                stateValue = value;

                StateChanged?.Invoke(stateValue);
            }
        }
    }
}