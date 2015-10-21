using System;
using System.Windows.Input;


namespace bytePassion.Lib.WpfLib.Commands
{

	public class ParameterrizedCommand<T> : ICommand, IDisposable
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

		private void CanExecuteChangedRequired (object sender, EventArgs eventArgs)
		{
			CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}

		public bool CanExecute (object parameter)
		{
			if (canExecute == null)
				return true;

			if (parameter != null)
				if (!(parameter is T))
					return false;

			return canExecute((parameter == null) ? default(T) : (T)parameter);
		}

		public bool CanExecute (T parameter)
		{
			return canExecute == null || canExecute(parameter);
		}

		public void Execute (object parameter)
		{
			execute((parameter == null) ? default(T) : (T)parameter);
		}

		public void Execute (T parameter)
		{
			execute(parameter);
		}

		public event EventHandler CanExecuteChanged;
	

		private bool disposed = false;
		public void Dispose ()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ParameterrizedCommand ()
		{
			Dispose(false);
		}

		private void Dispose (bool disposing)
		{
			if (!disposed)
				if (disposing)
					if (updateCommandInformation != null)
					{
						updateCommandInformation.UpdateOfCanExecuteChangedRequired -= CanExecuteChangedRequired;
						updateCommandInformation.Dispose();
					}

			disposed = true;
		}
	}
}

