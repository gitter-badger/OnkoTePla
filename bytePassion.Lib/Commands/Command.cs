using System;
using System.Windows.Input;

namespace bytePassion.Lib.Commands
{
	public class Command : ICommand, IDisposable
	{
		private readonly Func<bool> canExecute;
		private readonly Action execute;
		private readonly UpdateCommandInformation updateCommandInformation;


		public Command (Action execute, Func<bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public Command (Action execute, Func<bool> canExecute, UpdateCommandInformation updateCommandInformation)
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

			return canExecute();
		}

		public void Execute (object parameter)
		{
			execute();
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

		~Command ()
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
