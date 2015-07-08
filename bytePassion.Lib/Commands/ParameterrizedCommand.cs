using System;
using System.Windows.Input;


namespace bytePassion.Lib.Commands
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
			RaiseCanExecuteChanged();
		}

		public bool CanExecute (object parameter)
		{
			if (canExecute == null)
				return true;

			return canExecute((parameter == null) ? default(T) : (T)parameter);
		}

		public void Execute (object parameter)
		{
			execute((parameter == null) ? default(T) : (T)parameter);
		}

		public event EventHandler CanExecuteChanged;

		public void RaiseCanExecuteChanged ()
		{
			var handler = CanExecuteChanged;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

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

