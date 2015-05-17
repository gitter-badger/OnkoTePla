using System;
using System.Windows.Input;


namespace bytePassion.Lib.Commands
{

	public class ParameterrizedCommand<T> : ICommand
	{
		private readonly Predicate<T> canExecute;
		private readonly Action<T> execute;

	public ParameterrizedCommand (Action<T> execute, Predicate<T> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;	
		}

		public bool CanExecute(object parameter)
		{			
			if (canExecute == null)
				return true;

			return canExecute((parameter == null) ? default(T) : (T) Convert.ChangeType(parameter, typeof (T)));
		}

		public void Execute(object parameter)
		{
			execute((parameter == null) ? default(T) : (T) Convert.ChangeType(parameter, typeof (T)));
		}

		public event EventHandler CanExecuteChanged;

		public void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}
	}

}
