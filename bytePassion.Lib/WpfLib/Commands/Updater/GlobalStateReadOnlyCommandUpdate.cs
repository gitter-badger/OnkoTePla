using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using System;


namespace bytePassion.Lib.WpfLib.Commands.Updater
{

    public class GlobalStateReadOnlyCommandUpdate<T> : DisposingObject,
                                                       ICommandUpdater
    {
        public event EventHandler UpdateOfCanExecuteChangedRequired;

        private readonly ISharedStateReadOnly<T> sharedState;

        public GlobalStateReadOnlyCommandUpdate(ISharedStateReadOnly<T> sharedState)
        {
            this.sharedState = sharedState;

            sharedState.StateChanged += OnGlobalStateChanged;
        }

        private void OnGlobalStateChanged(T newValue)
        {
            UpdateOfCanExecuteChangedRequired?.Invoke(this, new EventArgs());
        }

        protected override void CleanUp()
        {
            sharedState.StateChanged -= OnGlobalStateChanged;
        }
    }
}