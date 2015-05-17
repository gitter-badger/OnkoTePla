using System;
using System.Windows.Input;


namespace bytePassion.Lib.Commands
{
	public class Command : ICommand
	{
		private readonly Func<bool> canExecute;
		private readonly Action execute;


		public Command (Action execute, Func<bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			if (canExecute == null)
				return true;

			return canExecute();
		}

		public void Execute(object parameter)
		{
			execute();
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
