using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using System;


namespace bytePassion.Lib.WpfLib.Commands.Updater
{

    public class GlobalStateCommandUpdate<T> : DisposingObject, 
                                               ICommandUpdater
    {
        public event EventHandler UpdateOfCanExecuteChangedRequired;

        private readonly IGlobalState<T> globalState;
        
        public GlobalStateCommandUpdate(IGlobalState<T> globalState)
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