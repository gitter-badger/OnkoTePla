using bytePassion.Lib.FrameworkExtensions;
using System;
using System.Windows.Input;


namespace bytePassion.Lib.WpfLib.Commands
{
    public class Command : DisposingObject, ICommand
    {
        private readonly Func<bool> canExecute;
        private readonly Action execute;
        private readonly UpdateCommandInformation updateCommandInformation;


        public Command(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public Command(Action execute, Func<bool> canExecute, UpdateCommandInformation updateCommandInformation)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.updateCommandInformation = updateCommandInformation;

            updateCommandInformation.UpdateOfCanExecuteChangedRequired += CanExecuteChangedRequired;
        }

        private void CanExecuteChangedRequired(object sender, EventArgs eventArgs)
        {
            RaiseCanExecuteChanged();
        }

        public bool CanExecute(object parameter = null)
        {
            if (canExecute == null)
                return true;

            return canExecute();
        }

        public void Execute(object parameter = null)
        {
            execute();
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public override void CleanUp()
        {
            if (updateCommandInformation != null)
            {
                updateCommandInformation.UpdateOfCanExecuteChangedRequired -= CanExecuteChangedRequired;
                updateCommandInformation.Dispose();
            }
        }
    }
}
