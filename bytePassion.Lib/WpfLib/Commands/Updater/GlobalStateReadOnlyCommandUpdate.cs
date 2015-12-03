using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using System;


namespace bytePassion.Lib.WpfLib.Commands.Updater
{

    public class GlobalStateReadOnlyCommandUpdate<T> : DisposingObject,
                                                       ICommandUpdater
    {
        public event EventHandler UpdateOfCanExecuteChangedRequired;

        private readonly IGlobalStateReadOnly<T> globalState;

        public GlobalStateReadOnlyCommandUpdate(IGlobalStateReadOnly<T> globalState)
        {
            this.globalState = globalState;

            globalState.StateChanged += OnGlobalStateChanged;
        }

        private void OnGlobalStateChanged(T newValue)
        {
            UpdateOfCanExecuteChangedRequired?.Invoke(this, new EventArgs());
        }

        protected override void CleanUp()
        {
            globalState.StateChanged -= OnGlobalStateChanged;
        }
    }
}