using bytePassion.Lib.FrameworkExtensions;
using System;
using System.Windows.Input;


namespace bytePassion.Lib.WpfLib.Commands
{

	public class ParameterrizedCommand<T> : DisposingObject, ICommand
	{
		private readonly Predicate<T> canExecute;
		private readonly Action<T> execute;
		private readonly UpdateCommandInformation updateCommandInformation;

		public ParameterrizedCommand (Action<T> execute, Predicate<T> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public ParameterrizedCommand (Action<T> execute, Predicate<T> canExecute,
									 UpdateCommandInformation updateCommandInformation)
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

        public void RaiseCanExecuteChanged()
        {           
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object parameter)
        {
            if (!(parameter is T))
                return false;

            if (canExecute == null)
                return true;

            return canExecute((T)parameter);
        }

        public bool CanExecute(T parameter)
        {
            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute((parameter == null) ? default(T) : (T)parameter);
        }

        public void Execute(T parameter)
        {
            execute(parameter);
        }                     		
		
		public event EventHandler CanExecuteChanged;	

	    public override void CleanUp()
	    {
            updateCommandInformation.UpdateOfCanExecuteChangedRequired -= CanExecuteChangedRequired;
            updateCommandInformation.Dispose();
        }
	   
	}
}

